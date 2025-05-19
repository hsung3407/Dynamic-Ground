using System;

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
            public Messages messages;
        }

        [Serializable]
        public class Messages
        {
            public Message[] messages;
        }
        
        [Serializable]
        public class Message
        {
            public string role;
            public string content;
        }
    }
}