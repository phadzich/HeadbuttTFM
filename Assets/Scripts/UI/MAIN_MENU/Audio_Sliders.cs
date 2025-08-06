using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider uiSlider;

    private void Start()
    {
        SetupSlider(musicSlider, "Music");
        SetupSlider(sfxSlider, "SFX");
        SetupSlider(ambientSlider, "Ambient");
        SetupSlider(uiSlider, "UI");
    }

    private void SetupSlider(Slider slider, string exposedParam)
    {
        // Load saved value or default to 1
        float savedValue = PlayerPrefs.GetFloat(exposedParam, 1f);
        slider.value = savedValue;

        // Apply initial volume
        SetVolume(exposedParam, savedValue);

        // Add listener to update volume in realtime
        slider.onValueChanged.AddListener((value) => {
            SetVolume(exposedParam, value);
            PlayerPrefs.SetFloat(exposedParam, value); // optional: save changes
        });
    }

    private void SetVolume(string exposedParam, float value)
    {
        try
        {
            float volumeInDb = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
            audioMixer.SetFloat(exposedParam, volumeInDb);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error al aplicar volumen para '{exposedParam}': {e.Message}");
        }
    }

}


