using PrimeTween;
using TMPro;
using UnityEngine;

public class DamageTakenIndicator : MonoBehaviour
{
    public TextMeshProUGUI amountText;

    private void Start()
    {
        ToggleIndicator(false);
    }
    public void AnimateDamage(int _amount)
    {

        ToggleIndicator(true);
        amountText.text = $"-{_amount}";
        Vector3 _growScale = new Vector3(.2f, .2f, 1);
        Tween.PunchScale(transform, _growScale, duration: 1f).OnComplete(()=>ToggleIndicator(false));
    }
    public void ToggleIndicator(bool _visible)
    {
        this.gameObject.SetActive(_visible);
    }
}
