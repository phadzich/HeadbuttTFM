using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    public Image brightnessOverlay; // Aca pongo la imagen negra
    public Slider brightnessSlider; // Slider de brillo

    private const string PlayerPrefKey = "BrightnessLevel";

    void Start()
    {
        // Cargo el valor guardado
        float savedValue = PlayerPrefs.GetFloat(PlayerPrefKey, 0.5f);
        brightnessSlider.value = savedValue;
        UpdateBrightness(savedValue);

        brightnessSlider.onValueChanged.AddListener(UpdateBrightness);
    }

    public void UpdateBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value; // Entre mas alto el slider, menos oscuro
            brightnessOverlay.color = c;
        }

        PlayerPrefs.SetFloat(PlayerPrefKey, value);
    }
}
