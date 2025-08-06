using UnityEngine;
using TMPro;

public class CameraShake_Script : MonoBehaviour
{
    public TMP_Dropdown cameraShakeDropdown;

    void Start()
    {
        int savedShake = PlayerPrefs.GetInt("CameraShakeEnabled", 1);

        
        cameraShakeDropdown.onValueChanged.RemoveListener(OnCameraShakeChanged);
        cameraShakeDropdown.value = savedShake;
        cameraShakeDropdown.onValueChanged.AddListener(OnCameraShakeChanged);
    }

    public void OnCameraShakeChanged(int value)
    {
        PlayerPrefs.SetInt("CameraShakeEnabled", value);
        PlayerPrefs.Save();
    }
}
