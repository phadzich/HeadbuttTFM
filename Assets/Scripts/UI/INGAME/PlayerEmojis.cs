using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEmojis : MonoBehaviour
{
    public Sprite failedSprite;
    public Sprite completeSprite;
    public Sprite damageSprite;
    public Sprite stunnedSprite;

    public Image emojiImage;

    private void Start()
    {
        this.transform.localScale = Vector3.zero;
    }
    public void FailedEmoji()
    {
        PopEmoji(failedSprite);
    }
    public void CompletedEmoji()
    {
        PopEmoji(completeSprite);
    }
    public void DamagedEmoji()
    {
        PopEmoji(damageSprite);
    }
    public void StunnedEmoji()
    {
        PopEmoji(stunnedSprite);
    }
    public void PopEmoji(Sprite _sprite)
    {
        emojiImage.sprite = _sprite;
        AnimateEmoji();

    }

    private void AnimateEmoji()
    {
        Tween.Scale(this.transform, startValue:Vector3.zero,endValue:Vector3.one, duration: 1f, ease:Ease.OutElastic).OnComplete(ReduceEmoji);
    }

    private void ReduceEmoji()
    {
        Tween.Scale(this.transform, startValue: Vector3.one, endValue: Vector3.zero, duration: .5f, ease: Ease.InExpo);
    }
}
