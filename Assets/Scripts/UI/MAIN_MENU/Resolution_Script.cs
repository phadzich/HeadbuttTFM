using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Resolution_Script : MonoBehaviour
{
    public static Resolution_Script Instance;

    public TMP_Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogError("El TMP_Dropdown no está asignado en el Inspector.");
            return;
        }

        availableResolutions = Screen.resolutions
    .OrderByDescending(r => r.width)
    .ThenByDescending(r => r.height)
    .ThenByDescending(r => r.refreshRateRatio.value)
    .ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            Resolution res = availableResolutions[i];
            string option = $"{res.width} x {res.height} @ {res.refreshRateRatio}Hz";
            options.Add(option);

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height &&
                    Mathf.Approximately((float)res.refreshRateRatio.value, (float)Screen.currentResolution.refreshRateRatio.value));
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // Cargo el índice guardado si existe, si no uso la actual
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        ChangeResolution(savedIndex);
    }

    void ChangeResolution(int index)
    {
        if (index < 0 || index >= availableResolutions.Length)
            return;

        Resolution res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRateRatio);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }
}
