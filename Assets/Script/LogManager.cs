using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class LogManager : MonoBehaviour
    {
        [SerializeField] private Log aiLog;
        [SerializeField] private Log playerLog;
        
        [SerializeField] private RectTransform contentRect;

        public void SetLog(string text, bool isPlayer)
        {
            var log = isPlayer ? playerLog : aiLog;
            log = Instantiate(log, contentRect);
            log.SetLog(text);

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        }
    }
}