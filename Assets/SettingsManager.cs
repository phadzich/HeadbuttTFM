using UnityEngine;

public class SettingsManager : MonoBehaviour
{

    public static SettingsManager instance;
    public LanguageHandler languageHandler;

    // Defaults
    //AUDIO
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float ambientVolume = 1f;
    private float UiVolume = 1f;

    //VIDEO
    private int resolutionIndex = 0;
    private int quality = 0;
    private int fullscreen = 1;
    private float brightness = 0;

    //ACCESIBILITY
    private int colorblindMode = 0;
    private float colorblindIntensity = 0;

    //GAMEPLAY
    private int language = 0; // 0 = inglés, 1 = español
    private int vibration = 1;
    public int shake = 1;


    private void Awake()
    {
        instance = this;
        Debug.Log("SettingsManager Awake@");
        LoadSettings();
        ApplyAll();
    }

    //SET AUDIO
    public void SetMasterVolume(float v)
    {
        masterVolume = v;
        SoundManager.instance.SetVolume("Master", v);
        PlayerPrefs.SetFloat("masterVolume", v);
    }
    public void SetMusicVolume(float v)
    {
        musicVolume = v;
        SoundManager.instance.SetVolume("Music", v);
        PlayerPrefs.SetFloat("musicVolume", v);
    }
    public void SetSfxVolume(float v)
    {
        sfxVolume = v;
        SoundManager.instance.SetVolume("SFX", v);
        PlayerPrefs.SetFloat("sfxVolume", v);
    }
    public void SetAmbientVolume(float v)
    {
        ambientVolume = v;
        SoundManager.instance.SetVolume("Ambient", v);
        PlayerPrefs.SetFloat("ambientVolume", v);
    }
    public void SetUIVolume(float v)
    {
        UiVolume = v;
        SoundManager.instance.SetVolume("UI", v);
        PlayerPrefs.SetFloat("uiVolume", v);
    }

    //SET VIDEO

    public void SetResolution(int index)
    {
        resolutionIndex = index;
        Resolution[] resolutions = Screen.resolutions;
        if (index >= 0 && index < resolutions.Length)
        {
            Resolution r = resolutions[index];
            Screen.SetResolution(r.width, r.height, fullscreen == 1);
        }
        PlayerPrefs.SetInt("resolutionIndex", index);
    }

    public void SetQuality(int q)
    {
        quality = q;
        QualitySettings.SetQualityLevel(q);
        PlayerPrefs.SetInt("quality", q);
    }

    public void SetFullscreen(bool state)
    {
        fullscreen = state ? 1 : 0;
        Screen.fullScreen = state;
        PlayerPrefs.SetInt("fullscreen", fullscreen);
    }

    public void SetBrightness(float v)
    {
        brightness = v;
        // Aquí aplicarías un material post-process, shader global, etc.
        PlayerPrefs.SetFloat("brightness", v);
    }

    public void SetColorblindMode(int mode)
    {
        colorblindMode = mode;
        PlayerPrefs.SetInt("colorblindMode", mode);
        // Aquí activarías tu shader de accesibilidad
    }

    public void SetColorblindIntensity(float v)
    {
        colorblindIntensity = v;
        PlayerPrefs.SetFloat("colorblindIntensity", v);
    }

    public void SetLanguage(int lang)
    {
        language = lang;
        PlayerPrefs.SetInt("language", lang);
        languageHandler.ChangeLocale(lang);
    }

    public void SetVibration(bool state)
    {
        vibration = state ? 1 : 0;
        PlayerPrefs.SetInt("vibration", vibration);
    }

    public void SetShake(bool state)
    {
        shake = state ? 1 : 0;
        PlayerPrefs.SetInt("shake", shake);
    }

    // ------------ LOAD / APPLY ALL ------------
    private void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        ambientVolume = PlayerPrefs.GetFloat("ambientVolume", 1f);
        UiVolume = PlayerPrefs.GetFloat("uiVolume", 1f);

        resolutionIndex = PlayerPrefs.GetInt("resolutionIndex", 0);
        quality = PlayerPrefs.GetInt("quality", 0);
        fullscreen = PlayerPrefs.GetInt("fullscreen", 1);
        brightness = PlayerPrefs.GetFloat("brightness", 0);

        colorblindMode = PlayerPrefs.GetInt("colorblindMode", 0);
        colorblindIntensity = PlayerPrefs.GetFloat("colorblindIntensity", 0);

        language = PlayerPrefs.GetInt("language", 0);
        vibration = PlayerPrefs.GetInt("vibration", 1);
        shake = PlayerPrefs.GetInt("shake", 1);
    }

    private void ApplyAll()
    {
        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);
        SetAmbientVolume(ambientVolume);
        SetUIVolume(UiVolume);

        SetResolution(resolutionIndex);
        SetQuality(quality);
        SetFullscreen(fullscreen == 1);
        SetBrightness(brightness);

        SetColorblindMode(colorblindMode);
        SetColorblindIntensity(colorblindIntensity);

        SetLanguage(language);
        SetVibration(vibration == 1);
        SetShake(shake == 1);
    }
}