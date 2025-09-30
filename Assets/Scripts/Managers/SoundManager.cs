using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public static class MixerGroups
{
    public const string Ambient = "Ambient";
    public const string Master = "Master";
    public const string Music = "Music";
    public const string SFX = "SFX";
    public const string UI = "UI";
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundEntry<MusicType>[] musicSoundList;
    [SerializeField] private SoundEntry<SFXType>[] sfxSoundList;
    [SerializeField] private SoundEntry<AmbientType>[] ambientSoundList;
    [SerializeField] private SoundEntry<UIType>[] UISoundList;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioSource sfx3D;
    [SerializeField] private AudioSource sfx2D;
    [SerializeField] private AudioSource sfxLoop;

    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(MusicType _sound, AudioClip _clip, float _volume = 1f)
        => instance.PlaySoundInternal(instance.musicSoundList, _sound, _volume, instance.musicSource, _clip, true);

    public static void PlaySound(SFXType _sound, float _volume = 1f, AudioClip _clip = null)
        => instance.PlaySoundInternal(instance.sfxSoundList, _sound, _volume, instance.sfx2D, _clip);

    public static void PlaySound(SFXType _sound, bool _loop, float _volume = 1f, AudioClip _clip = null)
        => instance.PlaySoundInternal(instance.sfxSoundList, _sound, _volume, instance.sfxLoop, _clip, _loop);

    public static void PlaySound(SFXType _sound, Vector3 _position, AudioClip _clip, float _volume = 1f, bool _loop = false)
        => instance.Play3DSoundInternal(instance.sfxSoundList, _sound, _volume, instance.sfx3D, _position, _clip, _loop);

    public static void PlaySound(AmbientType _sound, AudioClip _clip, float _volume = 1f)
        => instance.PlaySoundInternal(instance.ambientSoundList, _sound, _volume, instance.ambientSource, _clip, true);

    public static void PlaySound(UIType _sound, float _volume = 1f)
        => instance.PlaySoundInternal(instance.UISoundList, _sound, _volume, instance.uiSource);

    private void PlaySoundInternal<TEnum>(
        SoundEntry<TEnum>[] _list,
        TEnum _type,
        float _volume,
        AudioSource _source,
        AudioClip _clip = null,
        bool _loop = false
    ) where TEnum : Enum
    {

        AudioClip currentClip;

        var soundData = Array.Find(_list, s => s.type.Equals(_type));

        if(_type.Equals(SFXType.TIMERON))
        {
            Debug.Log(_type.ToString());
            Debug.Log(soundData.Clip);
        }

        if (_clip == null & soundData.Clip == null) return;

        currentClip = soundData.Clip;

        if (_clip != null)
        {
            currentClip = _clip;
        }

        _source.clip = currentClip;
        _source.loop = _loop;

        if (_loop) _source.Play();
        else _source.PlayOneShot(currentClip, _volume);
    }

    private void Play3DSoundInternal<TEnum>(
        SoundEntry<TEnum>[] _list,
        TEnum _type,
        float _volume,
        AudioSource _source,
        Vector3 _pos,
        AudioClip _clip = null,
        bool _loop = false
    ) where TEnum : Enum
    {
        AudioClip currentClip;

        var soundData = Array.Find(_list, s => s.type.Equals(_type));

        if (_clip == null & soundData.Clip == null) return;

        currentClip = soundData.Clip;

        if (_clip != null)
        {
            currentClip = _clip;
        }

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = _pos;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = currentClip;
        aSource.volume = _volume;
        aSource.spatialBlend = sfx3D.spatialBlend; // 3D
        aSource.outputAudioMixerGroup = sfx3D.outputAudioMixerGroup;

        aSource.loop = _loop;

        if (_loop) aSource.Play();
        else aSource.PlayOneShot(currentClip, _volume);

        Destroy(tempGO, currentClip.length);
    }

    public void StopAudioSource<TEnum>(TEnum _type) where TEnum : Enum
    {
        switch (_type)
        {
            case MusicType:
                musicSource.Stop();
                break;
            case AmbientType:
                ambientSource.Stop();
                break;
            case SFXType:
                sfxLoop.Stop();
                break;
            case UIType:
                uiSource.Stop();
                break;
        }
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
        string[] music = Enum.GetNames(typeof(MusicType));
        MusicType[] musicValues = (MusicType[])Enum.GetValues(typeof(MusicType));

        Array.Resize(ref musicSoundList, music.Length);
        for (int i = 0; i < music.Length; i++)
        {
            musicSoundList[i].name = music[i];
            musicSoundList[i].type = musicValues[i];
            musicSoundList[i].Category = SoundCategory.Music;
        }

        string[] sfx = Enum.GetNames(typeof(SFXType));
        SFXType[] sfxValues = (SFXType[])Enum.GetValues(typeof(SFXType));
        Array.Resize(ref sfxSoundList, sfx.Length);
        for (int i = 0; i < sfx.Length; i++)
        {
            sfxSoundList[i].name = sfx[i];
            sfxSoundList[i].type = sfxValues[i];
            sfxSoundList[i].Category = SoundCategory.SFX;
        }

        string[] ambient = Enum.GetNames(typeof(AmbientType));
        AmbientType[] ambientValues = (AmbientType[])Enum.GetValues(typeof(AmbientType));
        Array.Resize(ref ambientSoundList, ambient.Length);
        for (int i = 0; i < ambient.Length; i++)
        {
            ambientSoundList[i].name = ambient[i];
            ambientSoundList[i].type = ambientValues[i];
            ambientSoundList[i].Category = SoundCategory.Ambient;
        }

        string[] ui = Enum.GetNames(typeof(UIType));
        UIType[] uiValues = (UIType[])Enum.GetValues(typeof(UIType));
        Array.Resize(ref UISoundList, ui.Length);
        for (int i = 0; i < ui.Length; i++)
        {
            UISoundList[i].name = ui[i];
            UISoundList[i].type = uiValues[i];
            UISoundList[i].Category = SoundCategory.UI;
        }
    }
#endif
}

// Nos permite recibir cualquier tipo de enum para asi generalizar los parametros de las funciones
[Serializable]
public struct SoundEntry<TEnum> where TEnum : Enum
{
    [HideInInspector] public string name;
    public TEnum type;              // Ej: SFXType.MINEDCOMPLETE
    public AudioClip Clip => clip;       // clip para ese sonido
    public SoundCategory Category; // Categoria
    [SerializeField] private AudioClip clip;
}
