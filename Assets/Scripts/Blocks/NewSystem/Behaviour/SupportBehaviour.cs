using UnityEngine;

public class SupportBehaviour : MonoBehaviour, IBlockEffect
{

    public ParticleSystem damageParticles;

    public void Activate()
    {
        throw new System.NotImplementedException();
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
}
