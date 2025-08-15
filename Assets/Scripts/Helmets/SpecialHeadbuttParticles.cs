using UnityEngine;

public class SpecialHeadbuttParticles : MonoBehaviour
{
    float timeSincePlay;
    float maxDuration;

    public void Play()
    {

        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
        {
            if (particle.main.duration > maxDuration) maxDuration = particle.main.duration;
            particle.Play();
        }
    }

    private void Update()
    {
        timeSincePlay += Time.deltaTime;
        if(timeSincePlay >= maxDuration)
        {
            Destroy(this.gameObject);
        }
    }

}
