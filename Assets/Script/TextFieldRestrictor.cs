using System;
using TMPro;
using UnityEngine;

namespace Script
{
    public class TextFieldRestrictor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            Refresh();   
        }

        private void Refresh()
        {
            text.rectTransform.sizeDelta = new Vector2(Mathf.Min(800, text.preferredWidth + 20), text.rectTransform.sizeDelta.y);
        }
    }
}