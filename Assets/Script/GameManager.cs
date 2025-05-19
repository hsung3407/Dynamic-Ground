using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        private bool _isWaitResponse;
        
        [SerializeField] private LogManager logManager;

        public bool InputPrompt(string prompt)
        {
            if (_isWaitResponse) return false;
            _isWaitResponse = true;
            
            
            
            logManager.SetLog(prompt, true);
            return true;
        }

        private void ReceiveResponse(string response)
        {
            _isWaitResponse = false;
            logManager.SetLog(response, false);
        }
    }
}