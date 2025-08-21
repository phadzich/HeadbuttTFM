using PrimeTween;
using System;
using UnityEngine;

public class DirectionMoveEBehaviour : MonoBehaviour, IEnemyBehaviour {
    public float duration;
    public Vector3 direction;
    public float distance;
    public Ease ease;

    public void OnHit()
    {
        //
    }

    private void Start()
    {
        StartBehaviour();
    }

    public void StartBehaviour()
    {
        AnimateMovement();
    }

    private void AnimateMovement()
    {
        Vector3 _startPosition = this.transform.position;
        Vector3 _directionDistance = direction * distance;
        Vector3 _endPosition = _startPosition + _directionDistance;
        Tween.Position(this.transform,startValue:_startPosition, endValue:_endPosition,ease: ease, duration: duration);
    }

    public void StopBehaviour()
    {
        StopMovement();
    }

    private void StopMovement()
    {
        Tween.StopAll(this.transform);
    }
}