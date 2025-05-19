using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private float restrictWidth = 0;
    
    public void SetLog(string text)
    {
        tmp.text = text;
        var rect = GetComponent<RectTransform>();
        
        if(restrictWidth != 0) Restrict();
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public void Restrict()
    {
        tmp.rectTransform.sizeDelta = new Vector2(Mathf.Min(800, tmp.preferredWidth + 20), tmp.rectTransform.sizeDelta.y);
    }
}
