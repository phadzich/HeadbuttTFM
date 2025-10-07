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

    public Animator animator;
    void Start()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("OverlayDamage"), 1f);
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
                animator.SetBool("isWalking", true);
                break;
            case PlayerMainStateEnum.Walk:
                animator.SetBool("isWalking", true);
                break;

            case PlayerMainStateEnum.Disabled:
                animator.SetBool("isStunned", true);
                break;

            case PlayerMainStateEnum.FallingIntoMINE:
                animator.SetBool("isFalling", true);
                break;

            case PlayerMainStateEnum.FallingIntoNPC:
                animator.SetBool("isFalling", true);
                break;

            case PlayerMainStateEnum.Bouncing:
                animator.SetBool("isBouncing", true);
                break;

            case PlayerMainStateEnum.Headbutt:
                animator.ResetTrigger("isHeadbuttUsed");
                animator.SetTrigger("isHeadbuttUsed");
                break;

            case PlayerMainStateEnum.Dead:
                animator.SetBool("isDead", true);
                break;
        }
    }
    public void PlayStepAnimation()
    {
        if (isLeftStep)
        {
            animator.Play("StepLeft", 0, 0f); // Fuerza la animación desde el inicio
        }
        else
        {
            animator.Play("StepRight", 0, 0f);
        }

        // Alterna para la próxima llamada
        isLeftStep = !isLeftStep;
    }
    public void PlayHeadbuttAnimation()
    {
        animator.Play("HeadbuttAnimation", -1, 0f); // Cambia "HeadbuttAnimation" por el nombre real de tu animación de headbutt
        animator.SetTrigger("isHeadbuttUsed");
    }
    public void PlayDamageReaction()
    {
        print("Joma got hurt!");
        animator.SetTrigger("isHurt");
    }    
    private void ResetAllAnimationBools()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isBouncing", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isDead", false);
    }
}
