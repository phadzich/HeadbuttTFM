using UnityEngine;

public class SpecialHeadbuttParticles : MonoBehaviour
{
    public void Play()
    {
        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
        {
            particle.Play();
        }
    }
}
