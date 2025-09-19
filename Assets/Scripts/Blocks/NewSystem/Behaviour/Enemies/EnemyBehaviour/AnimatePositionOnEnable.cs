using PrimeTween;
using System;
using UnityEngine;


public class AnimatePositionOnEnable : MonoBehaviour
{
    public Vector3 originalPosition;
    public float duration;
    public float distance;
    public Vector3 direction;
    public Vector3 start;
    public Ease ease;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        AnimateUp();
    }

    private void AnimateUp()
    {
        Vector3 _startLocal = start;
        Vector3 _endPos = _startLocal + (direction*distance);
        Tween.LocalPosition(this.transform, duration: duration, startValue: _startLocal, endValue: _endPos, ease: ease);
    }
}
