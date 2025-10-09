using UnityEngine;
using PrimeTween;

[RequireComponent(typeof(RectTransform))]
public class BTNTooltipBounce : MonoBehaviour
{
    public float bounceHeight = 10f; // píxeles
    public float bounceDuration = 0.5f;

    RectTransform rt;
    Tween bounceTween;
    float startY;

    void Awake() => rt = GetComponent<RectTransform>();

    void OnEnable()
    {
        startY = rt.anchoredPosition.y;
        bounceTween = Tween.UIAnchoredPositionY(
            rt,
            endValue: startY + bounceHeight,
            duration: bounceDuration,
            cycles: -1,                    // infinito
            cycleMode: CycleMode.Yoyo,     // va y vuelve
            ease: Ease.InOutSine
        );
    }

    void OnDisable()
    {
        if (bounceTween.isAlive) bounceTween.Stop();
    }
}
