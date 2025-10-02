using UnityEngine;

public class SupportBehaviour : MonoBehaviour, IBlockBehaviour
{
    public AudioClip onBounceSound;
    public AudioClip onHeadbuttSound;
    public ParticleSystem feedbackParticles;


    public void OnBounced(HelmetInstance _helmetInstance)
    {

        MatchManager.Instance.FloorBounced();
        feedbackParticles.Play();
        SoundManager.PlaySound(SFXType.SUPPORT_BLOCK, 1f, onBounceSound);
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        feedbackParticles.Play();
        SoundManager.PlaySound(SFXType.SUPPORT_BLOCK, 1f, onHeadbuttSound);
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }
}
