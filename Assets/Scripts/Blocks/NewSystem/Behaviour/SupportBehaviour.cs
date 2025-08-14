using UnityEngine;

public class SupportBehaviour : MonoBehaviour, IBlockBehaviour
{

    public ParticleSystem damageParticles;

    public void Activate()
    {
         
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        damageParticles.Play();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        damageParticles.Play();
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }
}
