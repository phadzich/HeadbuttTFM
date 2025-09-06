using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrontEndFrame : MonoBehaviour
{
    public GameObject hotkeysBar;
    public TextMeshProUGUI frameTitleTXT;
    public TextMeshProUGUI frameDescriptionTXT;
    public Image frameIcon;
    public GameObject frameTitle;
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

        Tween.PositionY(frameTitle.transform,
            startValue: titleStartPos + 250,
            endValue: titleStartPos,
            duration: .8f,
            ease: Ease.InOutExpo);

        Tween.PositionY(hotkeysBar.transform,
            startValue: hotkeysStartPos - 150,
            endValue: hotkeysStartPos,
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

        Tween.PositionY(frameTitle.transform,
            startValue: titleStartPos,
            endValue: titleStartPos + 250,
            duration: .5f,
            ease: Ease.InOutExpo);

        Tween.PositionY(hotkeysBar.transform,
            startValue: hotkeysStartPos,
            endValue: hotkeysStartPos - 150,
            duration: .5f,
            ease: Ease.InOutExpo).OnComplete(DeactivateOnClose);
    }

    private void DeactivateOnClose()
    {
        Debug.Log("CHAU");
        //this.gameObject.SetActive(false);
    }

}
