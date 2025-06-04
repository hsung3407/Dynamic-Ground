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

                messages.messages.Insert(3,
                    new Schemas.Message()
                    {
                        role = "system",
                        content =
                            "출력: [2~6]의 출력 형식의 경우 무조건 특정 행동만을 할 수 있으며, 그 행동에 운 요소가 포함되거나 성공과 실패로 판가름 할 수 있을 경우에만 붙일 수 있다는 것을 명심하라"
                    });
                messages.messages.Insert(4,
                    new Schemas.Message()
                    {
                        role = "system",
                        content = "출력: [2~6]이 붙은 출력은 최대한 적게 나오도록 조절해야된다."
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

            reqMessages.Add(new Schemas.Message()
            {
                role = "user",
                content = "[2~6]이 20% 확률로 나오도록 해라"
            });

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