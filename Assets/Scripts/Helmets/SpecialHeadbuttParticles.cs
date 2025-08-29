using UnityEngine;

public class SpecialHeadbuttParticles : MonoBehaviour
{
    float timeSincePlay;
    float maxDuration;

    private Vector3 scaleFactor = Vector3.one;

    public void Play()
    {
        foreach (ParticleSystem _particle in GetComponentsInChildren<ParticleSystem>())
        {
            var _main = _particle.main;
            if (_main.duration > maxDuration)
                maxDuration = _main.duration;

            // Ajustar el shape del sistema
            var _shape = _particle.shape;
            Vector3 _original = _shape.scale;
            _shape.scale = new Vector3(
                _original.x * scaleFactor.x,
                _original.y * scaleFactor.y,
                _original.z * scaleFactor.z
            );

            _particle.Play();
        }
    }

    public void SetSize(Vector3 _size)
    {
        scaleFactor = _size;
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
