using Script.Networking;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        private bool _isWaitResponse;

        [SerializeField] private LogManager logManager;
        [SerializeField] private Model model;

        public bool InputPrompt(string prompt)
        {
            if (_isWaitResponse) return false;
            _isWaitResponse = true;

            model.Chat(prompt, ReceiveResponse);

            logManager.SetLog(prompt, true);
            return true;
        }

        private void ReceiveResponse(Schemas.ChatResponse response)
        {
            _isWaitResponse = false;
            logManager.SetLog(response.choices[0].message.content, false);
        }
    }
}