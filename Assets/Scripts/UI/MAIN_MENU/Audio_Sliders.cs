using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider uiSlider;

    private void Start()
    {
        SetupSlider(musicSlider, MixerGroups.Music);
        SetupSlider(sfxSlider, MixerGroups.SFX);
        SetupSlider(ambientSlider, MixerGroups.Ambient);
        SetupSlider(uiSlider, MixerGroups.UI);
    }

    private void SetupSlider(Slider slider, string exposedParam)
    {
        // Load saved value or default to 1
        float savedValue = PlayerPrefs.GetFloat(exposedParam, 1f);
        slider.value = savedValue;

        // Apply initial volume
        SoundManager.instance.SetVolume(exposedParam, savedValue);

        // Add listener to update volume in realtime
        slider.onValueChanged.AddListener((value) => {
            SoundManager.instance.SetVolume(exposedParam, value);
            PlayerPrefs.SetFloat(exposedParam, value); // optional: save changes
        });
    }

}


