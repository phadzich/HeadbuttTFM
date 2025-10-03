using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IBlockBehaviour
{
    public AudioClip onBounce;
    public AudioClip onHeadbutt;

    public ParticleSystem feedbackParticles;

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        PlayOnBounce();
        MatchManager.Instance.FloorBounced();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        PlayOnHB();
        MatchManager.Instance.FloorBounced();
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }

    private void PlayOnBounce()
    {
        if (feedbackParticles != null) feedbackParticles.Play();
        if (onBounce != null) SoundManager.PlaySound(SFXType.ENEMY, _clip: onBounce);
    }

    private void PlayOnHB()
    {
        if (feedbackParticles != null) feedbackParticles.Play();
        if (onHeadbutt != null) SoundManager.PlaySound(SFXType.ENEMY, _clip: onHeadbutt);
    }
}
