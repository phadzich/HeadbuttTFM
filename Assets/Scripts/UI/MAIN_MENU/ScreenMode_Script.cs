using UnityEngine;
using TMPro;

public class DisplayModeDropdown : MonoBehaviour
{
    public TMP_Dropdown modeDropdown;

    private void Start()
    {
        int savedMode = PlayerPrefs.GetInt("DisplayMode", 0);
        modeDropdown.value = savedMode;

        ApplyDisplayMode(savedMode);

        modeDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        ApplyDisplayMode(index);
        PlayerPrefs.SetInt("DisplayMode", index);
    }

    private void ApplyDisplayMode(int index)
    {
        switch (index)
        {
            case 0: // Fullscreen
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.ExclusiveFullScreen);
                Debug.Log("Mode: Fullscreen");
                break;
            case 1: // Window
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                Debug.Log("Mode: Window");
                break;
            default:
                Debug.LogWarning("Índice de modo de pantalla no reconocido");
                break;
        }
    }

    private void OnDestroy()
    {
        modeDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}
