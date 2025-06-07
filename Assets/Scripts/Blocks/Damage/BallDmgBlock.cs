using PrimeTween;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class BallDmgBlock : DamageBlock
{
    public float height;
    public float speed;
    public GameObject ball;
    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        Tween.LocalPositionY(ball.transform, endValue: height, duration: speed, ease: Ease.OutExpo, startDelay: Random.Range(0, .5f)).OnComplete(AnimateDown);
    }

    private void OnDisable()
    {
        Tween.StopAll(ball.transform);
    }

    void AnimateUp()
    {
        Tween.LocalPositionY(ball.transform, endValue: height, duration: speed, ease: Ease.OutExpo).OnComplete(AnimateDown);
    }

    void AnimateDown()
    {
        Tween.LocalPositionY(ball.transform, endValue: 0, duration: speed, ease: Ease.InExpo).OnComplete(AnimateUp);
    }

    public void PushPlayerTowardCenter(PlayerMovement _playerMovement)
    {
        Vector3 currentPos = _playerMovement.transform.position;
        Vector3 directionToCenter = (Vector3.zero - currentPos).normalized;

        _playerMovement.Knockback(directionToCenter);
    }

}
