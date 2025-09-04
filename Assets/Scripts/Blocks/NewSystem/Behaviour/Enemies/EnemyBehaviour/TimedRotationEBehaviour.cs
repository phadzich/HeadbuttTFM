using UnityEngine;
using System.Collections;
using System;
using PrimeTween;

public class TimedRotationBehaviour : MonoBehaviour, IEnemyBehaviour
{
    public float turnDuration;
    public float holdDuration;

    private EnemySFX sfx => GetComponent<EnemySFX>();

    public void StartBehaviour()
    {
        StartCoroutine(HoldTurn());
    }

    public void StopBehaviour()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartTurn()
    {
        //Debug.Log("StartTurn");
        Quaternion _startValue = transform.localRotation;
        Quaternion _endValue = _startValue * Quaternion.Euler(0f, 90f, 0f);
        Tween.LocalRotation(this.transform, duration: turnDuration, startValue: _startValue, endValue: _endValue);
        yield return new WaitForSeconds(turnDuration);
        StartCoroutine(HoldTurn());
    }

    private IEnumerator HoldTurn()
    {
        //Debug.Log("HoldTurn");
        yield return new WaitForSeconds(holdDuration);
        StartCoroutine(StartTurn()); 
    }


    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }

    public void OnHit()
    {
       //
    }
}
