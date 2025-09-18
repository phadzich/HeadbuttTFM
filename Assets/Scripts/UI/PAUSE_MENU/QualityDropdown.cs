using UnityEngine;
using TMPro;

public class QualityDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown qualityDropdown;

    private const string QualityPrefKey = "QualitySetting";

    void Start()
    {
        
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));

        // Cargo la calidad gráfica del QualityDropdown
        int savedQuality = PlayerPrefs.GetInt(QualityPrefKey, QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedQuality;
        qualityDropdown.RefreshShownValue();
        QualitySettings.SetQualityLevel(savedQuality, false);

        
        qualityDropdown.onValueChanged.AddListener(SetQualityLevelDropdown);
    }

    public void SetQualityLevelDropdown(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
        PlayerPrefs.SetInt(QualityPrefKey, index);
        PlayerPrefs.Save();
    }
}
