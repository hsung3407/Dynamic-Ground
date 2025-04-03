using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class PlayerPrompt : MonoBehaviour
    {
        [SerializeField] private TMP_InputField playerPrompt;
        private RectTransform _playerPromptRect;

        [SerializeField] private int heightExpendLimit = 5;
        private float _defaultHeight;

        private GameManager _gameManager;
        
        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            
            _playerPromptRect = playerPrompt.GetComponent<RectTransform>();
            _defaultHeight = _playerPromptRect.sizeDelta.y;
            playerPrompt.onValueChanged.RemoveAllListeners();
            playerPrompt.onValueChanged.AddListener(s =>
            {
                Debug.Log("Changed");
                var size = _playerPromptRect.sizeDelta;
                size.y = _defaultHeight * Mathf.Min(Regex.Matches(s, "\n").Count + 1, Mathf.Max(heightExpendLimit, 1));
                _playerPromptRect.sizeDelta = size;
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var input = playerPrompt.text;
                if (Input.GetKey(KeyCode.LeftShift) || input.Trim() == "" || _gameManager.InputPrompt(input)) return;
                playerPrompt.text = "";
            }
        }
    }
}