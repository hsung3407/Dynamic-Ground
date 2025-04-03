using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        private bool _isWaitResponse;
        
        public bool InputPrompt(string prompt)
        {
            if (_isWaitResponse) return false;

            
            
            return true;
        }
    }
}