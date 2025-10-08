using UnityEngine;

public class GroundAnimations : MonoBehaviour
{
    public Animator animator;

    public void Play(string animName)
    {
        animator.Play(animName, 0, 0f);
    }
}
