using PrimeTween;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogDialog : MonoBehaviour
{
    public TextMeshProUGUI logTXT;
    public Image logIcon;
    public CanvasGroup canvasGroup;

    public void ShowDialog(Sprite _icon, string _text)
    {
        logTXT.text = _text;
        logIcon.sprite = _icon;
        FadeOut();
    }
    
    public void FadeOut()
    {
        Tween.StopAll(canvasGroup);
        Tween.Alpha(canvasGroup, endValue: 0f, duration: .5f, startDelay: 2.5f).OnComplete(DestroyLog);
    }

    private void DestroyLog()
    {
        Destroy(this.gameObject);
    }
}
