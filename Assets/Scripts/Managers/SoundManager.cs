using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundCategory
{
    Music,
    SFX,
    Ambient,
    UI
}

public enum SoundType
{
    MINEDCOMPLETE,
    RESOURCEBOUNCE,
    FLOORBOUNCE,
    HEADBUTTFLOOR,
    HEADBUTTRESOURCE,
    COMBOFAIL,
    FIREDAMAGE,
    PUSHDAMAGE,
    ENEMYDAMAGE,
    SPIKEDAMAGE,
    RECIEVEDAMAGE,

    LEVELMUSIC,
    LEVELWATERMUSIC,
    LEVELAMBIENT,
    LEVELWATERAMBIENT,
    LEVELGRASSMUSIC,
    LEVELGRASSAMBIENT,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource uiSource;

    private static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        var soundData = instance.soundList[(int)sound];
        AudioClip randomClip = soundData.Sounds[UnityEngine.Random.Range(0, soundData.Sounds.Length)];

        switch (soundData.Category)
        {
            case SoundCategory.Music:
                instance.musicSource.clip = randomClip;
                instance.musicSource.volume = volume;
                instance.musicSource.loop = true;
                instance.musicSource.Play();
                break;
            case SoundCategory.SFX:
                instance.sfxSource.PlayOneShot(randomClip, volume);
                break;
            case SoundCategory.Ambient:
                instance.ambientSource.clip = randomClip;
                instance.ambientSource.volume = volume;
                instance.ambientSource.loop = true;
                instance.ambientSource.Play();
                break;
            case SoundCategory.UI:
                instance.uiSource.PlayOneShot(randomClip, volume);
                break;
        }
    }

    // 3D sound playback (e.g., explosions, spikes, etc.)
    public static void PlaySound3D(SoundType sound, Vector3 position, float volume = 1f)
    {
        var soundData = instance.soundList[(int)sound];
        if (soundData.Category != SoundCategory.SFX || soundData.Sounds.Length == 0) return;

        AudioClip clip = soundData.Sounds[UnityEngine.Random.Range(0, soundData.Sounds.Length)];
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    public void SetVolume(string mixerGroupName, float volume)
    {
        // volume should be from 0.0001 to 1.0; 0 mutes it entirely
        float dB = volume > 0 ? Mathf.Log10(volume) * 20f : -80f;
        audioMixer.SetFloat(mixerGroupName, dB);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < names.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]

public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    public SoundCategory Category => category;
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private SoundCategory category;

}
