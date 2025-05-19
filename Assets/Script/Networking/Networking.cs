using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Script.Networking
{
    public class Networking : MonoBehaviour
    {
        private static Networking _networking;
        //groq ai api key
        private const string URL = "https://api.groq.com/openai/v1/chat/completions";
        private const string APIKey = "";

        private void Awake()
        {
            Application.runInBackground = true;
            
            if (_networking != null)
                Destroy(_networking);
            _networking = this;
        }
        
        public class Request<T> where T : class
        {
            [CanBeNull] private Action<Schemas.ErrorBody> _errorAction;
            [CanBeNull] private Action<T> _responseAction;
            [CanBeNull] private Action _successAction;
            
            private readonly string _body;

            public Request(object body)
            {
                _body = JsonUtility.ToJson(body);
                Debugger.Log($"Added to body: {_body}");
            }

            public Request<T> OnError(Action<Schemas.ErrorBody> action)
            {
                _errorAction = action;
                return this;
            }

            public Request<T> OnSuccess(Action action)
            {
                _successAction = action;
                return this;
            }

            public Request<T> OnResponse(Action<T> action)
            {
                _responseAction += action;
                return this;
            }

            private UnityWebRequest WebRequest(string url)
            {
                return UnityWebRequest.Post(url, _body, "POST");
            }

            private IEnumerator _Request(string url)
            {
                Debugger.Log($"Sending Request to {url}");
                using var webRequest = WebRequest(url);
                webRequest.timeout = 15;
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader("Authorization", $"Bearer {APIKey}");
                Debugger.Log($"Headers: {webRequest.GetRequestHeader("Authorization")}");
                yield return webRequest.SendWebRequest();

                Debugger.Log($"ResponseCode: {webRequest.responseCode}");
                if (webRequest.downloadHandler == null) webRequest.downloadHandler = new DownloadHandlerBuffer();
                var bodyText = webRequest.downloadHandler.text;
                if (webRequest.result == UnityWebRequest.Result.Success)
                    if (webRequest.responseCode is >= 200 and <= 299)
                    {
                        Debugger.Log($"Response for {url}: {webRequest.responseCode}");
                        if (typeof(T) == typeof(void))
                            _responseAction?.Invoke(null);
                        else if (typeof(T) == typeof(string))
                            _responseAction?.Invoke(bodyText as T);
                        else
                            _responseAction?.Invoke(JsonConvert.DeserializeObject<T>(bodyText));
                        _successAction?.Invoke();
                
                        yield break;
                    }
                
                Debugger.Log($"Error Handled: {bodyText}");
                _errorAction?.Invoke(JsonConvert.DeserializeObject<Schemas.ErrorBody>(bodyText));
            }

            public void Build()
            {
                _networking.StartCoroutine(_Request(URL));
            }
        }
    }
}
