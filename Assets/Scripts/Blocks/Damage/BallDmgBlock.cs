using PrimeTween;

using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class BallDmgBlock : DamageBlock
{
    public float height;
    public float speed;
    public GameObject ball;
    public float holdDelay;
    public ParticleSystem fireParticles;
    

    private void Start()
    {
        Tween.LocalPositionY(ball.transform, endValue: height, duration: speed, ease: Ease.InExpo, startDelay: Random.Range(0, .5f)).OnComplete(HoldDown);
        fireParticles.Play();
    }

    private void OnDisable()
    {
        Tween.StopAll(ball.transform);
    }

    void AnimateUp()
    {
        fireParticles.Play();
        Tween.LocalPositionY(ball.transform, endValue: height, duration: speed, ease: Ease.InExpo).OnComplete(HoldDown);
    }

    void HoldDown()
    {
        fireParticles.Stop();
        Tween.LocalPositionY(ball.transform, startValue:.3f,endValue: .5f, duration: holdDelay, ease: Ease.InOutExpo).OnComplete(AnimateUp);
    }

}
