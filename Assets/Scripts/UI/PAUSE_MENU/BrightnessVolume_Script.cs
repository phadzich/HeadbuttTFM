using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BrightnessSlider : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Volume volume;

    private ColorAdjustments colorAdjustments;

    private const string BrightnessKey = "BrightnessValue";

    void Awake()
    {
        brightnessSlider.minValue = -3f;
        brightnessSlider.maxValue = 2f;
    }

    void Start()
    {
        // Aqui cargo el valor guardado
        float savedValue = PlayerPrefs.GetFloat(BrightnessKey, 0f);
        brightnessSlider.value = savedValue;


        // Aplio el ajuste del exposure
        if (volume.profile.TryGet(out colorAdjustments))
        {
            SetBrightness(savedValue);
        }

        // Cuando modifico el slider, modifico el brightness volume
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    public void SetBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat(BrightnessKey, value);
        }
    }
}
