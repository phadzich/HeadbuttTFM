using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PopupUI : MonoBehaviour
{
    public Image bgImage;
    public Image glowIMG;
    public Image itemIMG;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemAction;
    public Color helmetColor;
    public float holdDuration;

    public void ShowPopup(string _name, string _action, Sprite _img)
    {
        this.gameObject.SetActive(true);
        itemName.text = _name;
        itemAction.text = _action;
        itemIMG.sprite = _img;
        AnimateIn();
    }

    private void AnimateIn()
    {
        Tween.Alpha(bgImage, startValue: 0,
    endValue: 1,
    duration: .3f,
    ease: Ease.InOutExpo);
        Tween.Scale(itemIMG.transform, startValue: Vector3.zero, endValue: Vector3.one, duration: .3f, ease: Ease.OutBack);
        Tween.Scale(itemName.transform, startValue: Vector3.zero, endValue: Vector3.one, duration: .4f, ease: Ease.OutBack);
        Tween.Scale(itemAction.transform, startValue: Vector3.zero, endValue: Vector3.one, duration: .5f, ease: Ease.OutBack);
        Tween.Scale(glowIMG.transform, startValue: Vector3.zero, endValue: Vector3.one, duration: 1f, ease: Ease.OutElastic, endDelay:holdDuration).OnComplete(AnimateOut);
    }
    private void AnimateOut()
    {
        Tween.Alpha(bgImage, startValue: 1,
    endValue: 0,
    duration: .4f,
    ease: Ease.InOutExpo);
        Tween.Scale(itemIMG.transform, startValue: Vector3.one, endValue: Vector3.zero, duration: .4f, ease: Ease.InBack);
        Tween.Scale(itemName.transform, startValue: Vector3.one, endValue: Vector3.zero, duration: .3f, ease: Ease.InBack);
        Tween.Scale(itemAction.transform, startValue: Vector3.one, endValue: Vector3.zero, duration: .2f, ease: Ease.InBack);
        Tween.Scale(glowIMG.transform, startValue: Vector3.one, endValue: Vector3.zero, duration: .5f, ease: Ease.InElastic).OnComplete(Deactivate);
    }

    private void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
