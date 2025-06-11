using PrimeTween;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class BallDmgBlock : DamageBlock
{

    [Header("Ball")]
    public float height;
    public float speed;
    public GameObject ball;
    private AudioSource audioSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        Tween.LocalPositionY(ball.transform, endValue: height, duration: speed, ease: Ease.OutExpo, startDelay: Random.Range(0, .5f)).OnComplete(AnimateDown);
        audioSource = GetComponent<AudioSource>();
    }

    public override void Bounce()
    {
        audioSource.PlayOneShot(damageSound, 0.7f);
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
        audioSource.PlayOneShot(damageSound, 0.7f);
        _playerMovement.Knockback(directionToCenter);
    }

}
