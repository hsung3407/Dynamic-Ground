using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Script.Networking;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class Model : MonoBehaviour
    {
        [SerializeField] private string model = "llama-3.3-70b-versatile";

        private int _preSettingIndex = 0;
        private List<Schemas.Messages> _preSettingData = new List<Schemas.Messages>();

        [SerializeField] private int _talkLogLimitLength = 10000;
        private List<Schemas.Message> _talkLogData = new List<Schemas.Message>();
        
        public bool IsWait { get; private set; }
        
        private void Awake()
        {
            var preSets = Resources.LoadAll<TextAsset>("Data/PreSet");
            foreach (var preSet in preSets)
            {
                var messages = JsonConvert.DeserializeObject<Schemas.Messages>(preSet.text);
                messages.messages.Insert(0,
                    new Schemas.Message()
                    {
                        role = "system",
                        content = "너는 이 메시지 내용에 있는 system 컨텍스트와 예시 상호작용 대화 및 실제 상호작용 대화를 보고 의도된 분위기와 형식의 답변을 해야한다."
                    });
                _preSettingData.Add(messages);
            }
            // PreSetting();
        }
        
        public void Chat(string input, Action<Schemas.ChatResponse> onResponse)
        {
            while (_talkLogData.Sum(message => message.role.Length + message.content.Length) > _talkLogLimitLength)
            {
                _talkLogData.RemoveRange(0, 2);
            }
            
            var inputMessage = new Schemas.Message() { role = "user", content = input };
            _talkLogData.Add(inputMessage);

            var reqMessages = _preSettingData[Random.Range(0, _preSettingData.Count)].messages;
            reqMessages.AddRange(_talkLogData);

            var req = new Schemas.ChatRequest();
            req.model = model;
            req.messages = reqMessages;

            IsWait = true;
            API.Chat(req)
                .OnResponse(onResponse + (res =>
                {
                    _talkLogData.Add(res.choices[0].message);
                    IsWait = false;
                }))
                .Build();
        }

        // private void PreSetting()
        // {
        //     if (_preSettingIndex > _preSettingData.Count - 1)
        //     {
        //         IsWait = false;
        //         return;
        //     }
        //
        //     IsWait = true;
        //
        //     var req = new Schemas.ChatRequest();
        //     req.model = model;
        //     try
        //     {
        //         req.messages = _preSettingData[_preSettingIndex].messages;
        //
        //         API.Chat(req)
        //             .OnResponse(res =>
        //             {
        //                 Debug.Log($"Pre Setting Log {_preSettingIndex} : {res.choices[0]}");
        //                 _preSettingIndex++;
        //                 PreSetting();
        //             })
        //             .Build();
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }
    }
}