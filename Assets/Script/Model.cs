using System;
using System.Collections.Generic;
using System.Linq;
using Script.Networking;
using UnityEngine;

namespace Script
{
    public class Model : MonoBehaviour
    {
        [SerializeField] private string model = "llama-3.3-70b-versatile";

        private readonly List<Schemas.Message> _messages = new List<Schemas.Message>();

        public void Chat(string input, Action<Schemas.ChatResponse> onResponse)
        {
            var req = new Schemas.ChatRequest();
            req.model = model;
            _messages.Add(new Schemas.Message() { role = "user", content = input });
            req.messages = _messages;

            API.Chat(req)
                .OnResponse(onResponse + (res => _messages.Add(res.choices.Last()
                    .message)))
                .Build();
        }
    }
}