using Script.Networking;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LogManager logManager;
        [SerializeField] private Model model;

        public bool InputPrompt(string prompt)
        {
            if (model.IsWait) return false;

            model.Chat(prompt, ReceiveResponse);

            logManager.SetLog(prompt, true);
            return true;
        }

        private void ReceiveResponse(Schemas.ChatResponse response)
        {
            logManager.SetLog(response.choices[0].message.content, false);
        }
    }
}