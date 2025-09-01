using UnityEngine;

public class SupportBehaviour : MonoBehaviour, IBlockBehaviour
{

    public ParticleSystem feedbackParticles;

    public void Activate()
    {
         
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        feedbackParticles.Play();
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
