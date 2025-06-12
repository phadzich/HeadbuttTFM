using UnityEngine;

public class LevelAudio1 : MonoBehaviour
{
    [Header("Ambient Sound")]
    public AudioClip ambientClip;
    [Range(0f, 1f)] public float ambientVolume = 0.5f;

    [Header("OST / Music")]
    public AudioClip musicClip;
    [Range(0f, 1f)] public float musicVolume = 0.8f;

    private AudioSource ambientSource;
    private AudioSource musicSource;

    void Awake()
    {
        // Create two AudioSources for ambient and music
        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        // Setup Ambient AudioSource
        if (ambientClip != null)
        {
            ambientSource.clip = ambientClip;
            ambientSource.loop = true;
            ambientSource.volume = ambientVolume;
            ambientSource.playOnAwake = false;
            ambientSource.spatialBlend = 0f; // 2D sound
            ambientSource.Play();
        }

        // Setup Music AudioSource
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.playOnAwake = false;
            musicSource.spatialBlend = 0f; // 2D sound
            musicSource.Play();
        }
    }
}