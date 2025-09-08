using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrontEndFrame : MonoBehaviour
{
    public RectTransform hotkeysBar;
    public TextMeshProUGUI frameTitleTXT;
    public TextMeshProUGUI frameDescriptionTXT;
    public Image frameIcon;
    public RectTransform frameTitle;
    public float hotkeysStartPos;
    public float titleStartPos;
    public Image frameBG;

    private void Start()
    {
        hotkeysStartPos = hotkeysBar.transform.position.y;
        titleStartPos = frameTitle.transform.position.y;
        CloseFrame();
    }

    public void OpenFrame(string _title, string _descrip, Sprite _icon)
    {
        this.gameObject.SetActive(true);
        //DATA
        frameIcon.sprite = _icon;
        frameTitleTXT.text = _title;
        frameDescriptionTXT.text = _descrip;

        //ANIMAR
        Tween.Alpha(frameBG, startValue: 0,
            endValue: 1,
            duration: .3f,
            ease: Ease.InOutExpo);

        Tween.UIAnchoredPositionY(frameTitle,
            startValue: 350,
            endValue: 0,
            duration: .8f,
            ease: Ease.InOutExpo);

        Tween.UIAnchoredPositionY(hotkeysBar,
            startValue: -250,
            endValue: 50,
            duration: .8f,
            ease: Ease.InOutExpo);
    }

    public void CloseFrame()
    {
        //ANIMAR
        Tween.Alpha(frameBG, startValue: 1,
            endValue: 0,
            duration: .3f,
            ease: Ease.InOutExpo);

        Tween.UIAnchoredPositionY(frameTitle,
            startValue: 0,
            endValue: 350,
            duration: .5f,
            ease: Ease.InOutExpo);

        Tween.UIAnchoredPositionY(hotkeysBar,
            startValue: 50,
            endValue:  -250,
            duration: .5f,
            ease: Ease.InOutExpo).OnComplete(DeactivateOnClose);
    }

    private void DeactivateOnClose()
    {
        Debug.Log("CHAU");
        //this.gameObject.SetActive(false);
    }

}
