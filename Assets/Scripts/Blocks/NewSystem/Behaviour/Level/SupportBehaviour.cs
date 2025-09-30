using UnityEngine;

public class SupportBehaviour : MonoBehaviour, IBlockBehaviour
{
    public AudioClip onBounceSound;
    public ParticleSystem feedbackParticles;

    public AudioClip idleSound;
    public bool canBeBounce;

    public void Activate()
    {
         if (idleSound != null)
        {
            SoundManager.PlaySound(SFXType.SUPPORTBLOCK, true, _clip: idleSound);
        }
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        if (!canBeBounce) return;

        MatchManager.Instance.FloorBounced();
        feedbackParticles.Play();
        SoundManager.PlaySound(SFXType.SUPPORTBLOCK, 1f, onBounceSound);
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        if (!canBeBounce) return;

        MatchManager.Instance.FloorBounced();
        feedbackParticles.Play();
        SoundManager.PlaySound(SFXType.SUPPORTBLOCK, 1f, onBounceSound);
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }
}
