using System;
using System.Collections;
using Script.Networking;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private LogManager logManager;
        [SerializeField] private Model model;
        
        private bool _started = false;

        private void Start()
        {
            StartCoroutine(StartSequence());
        }

        private IEnumerator StartSequence()
        {
            yield return new WaitUntil(() => model.IsWait);
            model.Chat("랜덤한 배경과 캐릭터, 설정과 세계관으로 이루어진 TRPG 게임을 형식대로 시작하라", res =>
            {
                ReceiveResponse(res);
                _started = true;
            });
        }

        public bool InputPrompt(string prompt)
        {
            if (model.IsWait || !_started) return false;

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