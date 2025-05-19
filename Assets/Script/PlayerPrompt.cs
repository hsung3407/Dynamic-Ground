using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script
{
    public class PlayerPrompt : MonoBehaviour
    {
        [SerializeField] private TMP_InputField playerPrompt;
        private RectTransform _playerPromptRect;

        [SerializeField] private int heightExpendLimit = 5;
        private float _defaultHeight;

        [SerializeField] private GameManager gameManager;

        private bool _refreshFlag;

        private void Start()
        {
            _playerPromptRect = playerPrompt.GetComponent<RectTransform>();
            _defaultHeight = _playerPromptRect.sizeDelta.y;
            playerPrompt.onValueChanged.RemoveAllListeners();
            playerPrompt.onValueChanged.AddListener(_ => { _refreshFlag = true; });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var input = playerPrompt.text;
                if (input.Trim() == "" || !gameManager.InputPrompt(input)) return;
                playerPrompt.text = "";
            }
        }

        private void LateUpdate()
        {
            if (_refreshFlag)
            {
                _refreshFlag = false;

                var size = _playerPromptRect.sizeDelta;
                var max = _defaultHeight * heightExpendLimit;
                var preferredHeight = playerPrompt.preferredHeight;
                var difference = preferredHeight % _defaultHeight;
                var dynamicalHeight = preferredHeight % _defaultHeight > _defaultHeight / 2
                    ? preferredHeight + difference
                    : preferredHeight - difference;
                size.y = Mathf.Clamp(dynamicalHeight, _defaultHeight, max);
                _playerPromptRect.sizeDelta = size;
            }
        }
    }
}