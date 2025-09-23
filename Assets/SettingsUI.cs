using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("TABS")]
    public Transform panels;
    public GameObject gameplayPanel;
    public GameObject videoPanel;
    public GameObject audioPanel;
    public GameObject accessPanel;
    public GameObject controlsPanel;


    [Header("VIDEO")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider contrastSlider;

    [Header("COLORBLIND")]
    [SerializeField] private TMP_Dropdown colorblindDropdown;
    [SerializeField] private Slider colorblindSlider;

    [Header("AUDIO")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    private const float minBrightness = -3f;
    private const float maxBrightness = 2f;
    private const float minContrast = -50f;
    private const float maxContrast = 50f;

    private Resolution[] resolutions;

    void Start()
    {
        PopulateResolutions();
        PopulateQualities();
        PopulateColorBlindModes();
        PopulateSliders();
    }

    public void OpenTab(GameObject _panel)
    {
        foreach(Transform _child in panels)
        {
            _child.gameObject.SetActive(false);
        }
        _panel.SetActive(true);

    }

    private void PopulateQualities()
    {
        // Llenar opciones
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));

        // Obtener valor guardado o default en High (el último)
        int savedIndex = PlayerPrefs.GetInt("qualityIndex", -1);

        int indexToUse = savedIndex >= 0 ? savedIndex : QualitySettings.names.Length - 1;

        qualityDropdown.value = indexToUse;
        qualityDropdown.RefreshShownValue();

        // Aplicar calidad
        QualitySettings.SetQualityLevel(indexToUse, true);
    }

    private void PopulateResolutions()
    {
        // Filtramos duplicados y nos quedamos con la mayor frecuencia por tamaño
        resolutions = Screen.resolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.OrderByDescending(r => r.refreshRateRatio.value).First())
            .ToArray();

        resolutionDropdown.ClearOptions();

        // Lista de opciones (agregamos "NATIVA" en el index 0)
        var options = new System.Collections.Generic.List<string>();
        // Primera opción: Nativa como Recommended
        Resolution native = Screen.currentResolution;
        options.Add($"Recommended ({native.width} x {native.height})");
        options.AddRange(resolutions.Select(r => $"{r.width} x {r.height}"));

        resolutionDropdown.AddOptions(options);

        // Si existe PlayerPrefs, cargamos. Si no, default = NATIVA (index 0).
        int useIndex = PlayerPrefs.GetInt("resolutionIndex", 0);

        resolutionDropdown.value = useIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SettingsManager.instance.SetResolution);
    }

    private void PopulateColorBlindModes()
    {

        // Obtener valor guardado o default en High (el último)
        int savedIndex = PlayerPrefs.GetInt("colorblindMode", 0);

        colorblindDropdown.value = savedIndex;
        colorblindDropdown.RefreshShownValue();

    }

    public void PopulateSliders()
    {
        // Brillo
        float savedBrightness = PlayerPrefs.GetFloat("brightness", 0f);

        // Mapear de rango
        brightnessSlider.value = Mathf.InverseLerp(minBrightness, maxBrightness, savedBrightness) * 2f - 1f;
        brightnessSlider.onValueChanged.AddListener((v) =>
        {
            float mapped = Mathf.Lerp(minBrightness, maxBrightness, (v + 1f) / 2f);
            SettingsManager.instance.SetBrightness(mapped);
        });

        // Contraste
        float savedContrast = PlayerPrefs.GetFloat("contrast", 0f);
        contrastSlider.value = Mathf.InverseLerp(minContrast, maxContrast, savedContrast) * 2f - 1f;
        contrastSlider.onValueChanged.AddListener((v) =>
        {
            float mapped = Mathf.Lerp(minContrast, maxContrast, (v + 1f) / 2f);
            SettingsManager.instance.SetContrast(mapped);
        });

        //ColorblindIntensity
        float savedIntensity = PlayerPrefs.GetFloat("colorblindIntensity", 1f);
        colorblindSlider.value = savedIntensity;

        //MIXERS
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        ambientSlider.value = PlayerPrefs.GetFloat("ambientVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        uiSlider.value = PlayerPrefs.GetFloat("uiVolume", 1f);
    }

}
