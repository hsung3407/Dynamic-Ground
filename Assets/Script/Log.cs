using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    
    public void SetLog(string text)
    {
        tmp.text = text;
        var rect = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }
}
