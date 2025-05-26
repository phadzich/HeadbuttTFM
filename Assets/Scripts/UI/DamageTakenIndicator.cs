using PrimeTween;
using UnityEngine;

public class DamageTakenIndicator : MonoBehaviour
{

    private void Start()
    {
        ToggleIndicator(false);
    }
    public void AnimateDamage()
    {
        ToggleIndicator(true);
        Vector3 _growScale = new Vector3(.2f, .2f, 1);
        Tween.PunchScale(transform, _growScale, duration: 1f).OnComplete(()=>ToggleIndicator(false));
    }
    public void ToggleIndicator(bool _visible)
    {
        this.gameObject.SetActive(_visible);
    }
}
