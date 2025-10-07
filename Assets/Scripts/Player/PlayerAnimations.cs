using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private bool isLeftStep = true;

    [SerializeField]
    public GameObject bodyMesh;
    public float headbuttSquashRatio = .4f;
    public float headbuttSquashDuration = 1f;
    public float bounceSquashRatio = .3f;
    public float bounceSquashDuration = .3f;

    public Animator animatorPlayer, faceAnimator;
    void Start()
    {
        animatorPlayer.SetLayerWeight(animatorPlayer.GetLayerIndex("OverlayDamage"), 1f);

        faceAnimator.SetLayerWeight(faceAnimator.GetLayerIndex("OverlayDamage"), 1f);
    }
    public void RotateBody(float _degrees)
    {
        var finalRotation = new Vector3(_degrees, 0, 0);
        Tween.LocalRotation(bodyMesh.transform, endValue: finalRotation, duration: .5f, ease: Ease.InOutExpo);
    }

    public void HeadbuttSS()
    {
        var startScale = new Vector3(1 + headbuttSquashRatio, headbuttSquashRatio, 1 + headbuttSquashRatio);
        var endScale = new Vector3(1, 1, 1);
        var startRotation = new Vector3(-180, 0, 0);
        var endRotation = new Vector3(0, 0, 0);
        Tween.Scale(bodyMesh.transform, startValue: startScale, endValue: endScale, duration: headbuttSquashDuration, ease: Ease.OutBack);
        //Tween.Rotation(bodyMesh.transform, startValue: startRotation, endValue: endRotation, duration: 2f, ease: Ease.InOutExpo).OnComplete(() => RotateBody(180));
        //Tween.EulerAngles(bodyMesh.transform, startValue: startRotation, endValue: new Vector3(+180, 0,0), duration: 2);
    }

    public void BounceSS()
    {
        var startScale = new Vector3(1 + bounceSquashRatio, bounceSquashRatio, 1 + bounceSquashRatio);
        var endScale = new Vector3(1, 1, 1);
        Tween.Scale(bodyMesh.transform, startValue: startScale, endValue: endScale, duration: bounceSquashDuration, ease: Ease.OutSine);
    }

    public void PlayStateAnimation(PlayerMainStateEnum state)
    {
        // Resetear todos los parámetros relevantes
        ResetAllAnimationBools();

        switch (state)
        {
            case PlayerMainStateEnum.Idle:
                animatorPlayer.SetBool("isWalking", true);

                faceAnimator.SetBool("isWalking", true);
                break;
            case PlayerMainStateEnum.Walk:
                animatorPlayer.SetBool("isWalking", true);

                faceAnimator.SetBool("isWalking", true);
                break;

            case PlayerMainStateEnum.Disabled:
                animatorPlayer.SetBool("isStunned", true);

                faceAnimator.SetBool("isStunned", true);
                break;

            case PlayerMainStateEnum.FallingIntoMINE:
                animatorPlayer.SetBool("isFalling", true);

                faceAnimator.SetBool("isFalling", true);
                break;

            case PlayerMainStateEnum.FallingIntoNPC:
                animatorPlayer.SetBool("isFalling", true);

                faceAnimator.SetBool("isFalling", true);
                break;

            case PlayerMainStateEnum.Bouncing:
                animatorPlayer.SetBool("isBouncing", true);

                faceAnimator.SetBool("isBouncing", true);
                break;

            case PlayerMainStateEnum.Headbutt:
                /*animatorPlayer.ResetTrigger("isHeadbuttUsed");
                faceAnimator.ResetTrigger("isHeadbuttUsed");

                animatorPlayer.SetTrigger("isHeadbuttUsed");                
                faceAnimator.SetTrigger("isHeadbuttUsed");*/
                break;

            case PlayerMainStateEnum.Dead:
                animatorPlayer.SetBool("isDead", true);

                faceAnimator.SetBool("isDead", true);
                break;
        }
    }
    public void PlayStepAnimation()
    {
        if (isLeftStep)
        {
            animatorPlayer.Play("StepLeft", 0, 0f); // Fuerza la animación desde el inicio

        }
        else
        {
            animatorPlayer.Play("StepRight", 0, 0f);

        }

        // Alterna para la próxima llamada
        isLeftStep = !isLeftStep;
    }
    public void PlayHeadbuttAnimation()
    {
        animatorPlayer.Play("HeadbuttAnimation", -1, 0f);
        //animatorPlayer.SetTrigger("isHeadbuttUsed");

        faceAnimator.Play("HeadbuttFaceAnimation", -1, 0f);
        //faceAnimator.SetTrigger("isHeadbuttUsed");
    }
    public void PlayDamageReaction()
    {
        print("Joma got hurt!");
        animatorPlayer.SetTrigger("isHurt");

        faceAnimator.SetTrigger("isFaceHurt");
    }
    private void ResetAllAnimationBools()
    {
        animatorPlayer.SetBool("isWalking", false);
        animatorPlayer.SetBool("isBouncing", false);
        animatorPlayer.SetBool("isStunned", false);
        animatorPlayer.SetBool("isFalling", false);
        animatorPlayer.SetBool("isDead", false);

        faceAnimator.SetBool("isWalking", false);
        faceAnimator.SetBool("isBouncing", false);
        faceAnimator.SetBool("isStunned", false);
        faceAnimator.SetBool("isFalling", false);
        faceAnimator.SetBool("isDead", false);        
    }
}
