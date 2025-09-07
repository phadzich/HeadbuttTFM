using UnityEngine;

public class SupportBehaviour : MonoBehaviour, IBlockBehaviour
{
    public AudioClip onBounceSound;
    public ParticleSystem feedbackParticles;

    public void Activate()
    {
         
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        feedbackParticles.Play();
        Debug.Log("wate sound:");
        Debug.Log(onBounceSound);
        SoundManager.PlaySound(SFXType.SUPPORTBLOCK, 1f, onBounceSound);
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        feedbackParticles.Play();
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }
}
