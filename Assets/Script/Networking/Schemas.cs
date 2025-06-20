using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Script.Networking
{
    public class Schemas
    {
        [Serializable]
        public class ErrorBody
        {
            public int errorId;
            public string message;
        }

        [Serializable]
        public class ChatRequest
        {
            public string model;
            public List<Message> messages;
        }

        [Serializable]
        [JsonObject("messages")]
        public class Messages
        {
            public List<Message> messages;
        }

        [Serializable]
        [JsonObject("message")]
        public class Message
        {
            public string role;
            public string content;
        }

        [Serializable]
        public class ChatResponse
        {
            public string id;
            [JsonProperty("object")] public string obj;
            public long created;
            public string model;
            public List<Choice> choices;
            public Usage usage;
            public string system_fingerprint;
            public XGroq x_groq;
        }

        [Serializable]
        [JsonObject("usage")]
        public class Usage
        {
            public double queue_ime;
            public long prompt_tokens;
            public double prompt_time;
            public long completion_tokens;
            public double completion_time;
            public long total_tokens;
            public double total_time;
        }

        [Serializable]
        public class Choice
        {
            public long index;
            public Message message;
            public string logprobs;
            public string finish_reason;
        }

        [Serializable]
        [JsonObject("x_groq")]
        public class XGroq
        {
            public string id;
        }
    }
}