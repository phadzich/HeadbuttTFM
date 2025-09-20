using UnityEngine;
using TMPro;

public class DisplayModeDropdown : MonoBehaviour
{
    /*
    public static DisplayModeDropdown Instance;

    public TMP_Dropdown modeDropdown;

    private void Awake()
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

    private void Start()
    {
        SetupDropdown();
    }

    public void SetupDropdown()
    {
        if (modeDropdown == null)
        {
            modeDropdown = FindFirstObjectByType<TMP_Dropdown>();

            if (modeDropdown == null)
            {
                Debug.LogWarning("No se encontró el Dropdown de modo de pantalla.");
                return;
            }
        }

        int savedMode = PlayerPrefs.GetInt("DisplayMode", 0);
        modeDropdown.value = savedMode;
        modeDropdown.RefreshShownValue();

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
                //Debug.Log("Mode: Fullscreen");
                break;
            case 1: // Windowed
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                //Debug.Log("Mode: Window");
                break;
            default:
                //Debug.LogWarning("Índice de modo de pantalla no reconocido");
                break;
        }
    }

    private void OnDestroy()
    {
        if (modeDropdown != null)
            modeDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
    */
}
