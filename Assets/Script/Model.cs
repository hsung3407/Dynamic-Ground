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

        public bool IsWait { get; private set; }

        public void Chat(string input, Action<Schemas.ChatResponse> onResponse)
        {
            var req = new Schemas.ChatRequest();
            req.model = model;
            req.messages = new List<Schemas.Message>() { new Schemas.Message() { role = "user", content = input } };

            IsWait = true;
            API.Chat(req)
                .OnResponse(onResponse + (res => IsWait = false))
                .Build();
        }
    }
}