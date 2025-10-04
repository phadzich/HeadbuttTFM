using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsManager : MonoBehaviour
{

    public static SettingsManager instance;
    public LanguageHandler languageHandler;
    public ColorBlindHandler colorBlindHandler;
    public SettingsUI settingsUI;
    public PauseHandler pauseHandler;

    [SerializeField] private Volume settingsVolume;
    private ColorAdjustments colorAdjustments;

    // Defaults
    //AUDIO
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float ambientVolume = 1f;
    private float UiVolume = 1f;

    //VIDEO
    private int resolutionIndex = 0;
    private int qualityIndex = 0;
    private int fullscreen = 1;
    private float brightness = 0;
    private float contrast = 0;

    //ACCESIBILITY
    private int colorblindMode = 0;
    private float colorblindIntensity = 1;
    private int combatlog = 1;

    //GAMEPLAY
    private int language = 0; // 0 = inglés, 1 = español
    private int vibration = 1;
    public int shake = 1;

    public Resolution[] GetFilteredResolutions()
    {
        // Si tu Unity no soporta refreshRateRatio, sustituye por r.refreshRate (pero dijiste usar lo moderno)
        return Screen.resolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.OrderByDescending(r => r.refreshRateRatio.value).First())
            .ToArray();
    }

    private void Awake()
    {
        instance = this;
        Debug.Log("SettingsManager Awake@");

        if (settingsVolume != null && settingsVolume.profile != null)
        {
            // Busca el override de ColorAdjustments en tu SettingsVolume
            if (!settingsVolume.profile.TryGet(out colorAdjustments))
            {
                Debug.LogWarning("El Volume no tiene Color Adjustments agregado.");
            }
        }

        LoadSettings();
        ApplyAll();
    }

    private void Start()
    {
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

        Resolution r;

        if (index == 0)
        {
            // Recommended / nativa
            r = Screen.currentResolution;
        }
        else
        {
            // usamos la lista filtrada para que coincida exactamente con el dropdown
            var filtered = GetFilteredResolutions();
            int safeIndex = Mathf.Clamp(index - 1, 0, filtered.Length - 1);
            r = filtered[safeIndex];
        }

        // Aplica según el estado actual de fullscreen
        ApplyResolution(r.width, r.height);

        // Guardar el índice del dropdown (0 = recommended)
        PlayerPrefs.SetInt("resolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void SetQuality(int q)
    {
        qualityIndex = q;
        QualitySettings.SetQualityLevel(q, true);
        PlayerPrefs.SetInt("qualityIndex", q);
    }

    public void SetFullscreen(bool state)
    {
        fullscreen = state ? 1 : 0;
        PlayerPrefs.SetInt("fullscreen", fullscreen);
        PlayerPrefs.Save();

        // Reaplica la resolución actual (según resolutionIndex)
        if (resolutionIndex == 0)
        {
            Resolution native = Screen.currentResolution;
            ApplyResolution(native.width, native.height);
        }
        else
        {
            var filtered = GetFilteredResolutions();
            int safeIndex = Mathf.Clamp(resolutionIndex - 1, 0, filtered.Length - 1);
            Resolution r = filtered[safeIndex];
            ApplyResolution(r.width, r.height);
        }
    }

    private void ApplyResolution(int width, int height)
    {
        if (fullscreen == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(width, height, true);
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(width, height, false);
        }

        
    }

    public void SetBrightness(float v)
    {
        if (colorAdjustments != null)
        {
            float brightnessValue = Mathf.Lerp(-3f, 2f, (v + 1f) / 2f);
            colorAdjustments.postExposure.value = brightnessValue;
            PlayerPrefs.SetFloat("brightness", v);
        }
    }

    public void SetContrast(float v)
    {
        if (colorAdjustments != null)
        {
            float contrastValue = Mathf.Lerp(-50f, 50f, (v + 1f) / 2f);
            colorAdjustments.contrast.value = contrastValue;
            PlayerPrefs.SetFloat("contrast", v);
        }
    }

    public void ResetBrightnessContrast()
    {
        SetContrast(0);
        SetBrightness(0);
        settingsUI.PopulateSliders();
    }

    public void SetColorblindMode(int mode)
    {
        colorblindMode = mode;
        PlayerPrefs.SetInt("colorblindMode", mode);
        colorBlindHandler.ApplyLUT(mode);
    }

    public void SetColorblindIntensity(float v)
    {
        colorblindIntensity = v;
        colorBlindHandler.SetContribution(v);
        PlayerPrefs.SetFloat("colorblindIntensity", v);
    }

    public void SetLanguage(int lang)
    {
        language = lang;
        PlayerPrefs.SetInt("language", lang);
        languageHandler.ChangeLocale(lang);
    }

    public void SetCombatLog(bool v)
    {
        combatlog = v ? 1 : 0;

        PlayerPrefs.SetInt("combatLog", combatlog);
        CombatLogHUD.Instance.logHUD.SetActive(v);
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
        qualityIndex = PlayerPrefs.GetInt("qualityIndex", 0);
        fullscreen = PlayerPrefs.GetInt("fullscreen", 1);
        brightness = PlayerPrefs.GetFloat("brightness", 0);
        contrast = PlayerPrefs.GetFloat("contrast", 0);

        colorblindMode = PlayerPrefs.GetInt("colorblindMode", 0);
        colorblindIntensity = PlayerPrefs.GetFloat("colorblindIntensity", 1);
        combatlog = PlayerPrefs.GetInt("combatlog", 1);

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
        SetQuality(qualityIndex);
        SetFullscreen(fullscreen == 1);
        SetBrightness(brightness);
        SetContrast(contrast);

        SetColorblindMode(colorblindMode);
        SetColorblindIntensity(colorblindIntensity);

        SetLanguage(language);
        SetVibration(vibration == 1);
        SetShake(shake == 1);
    }
}