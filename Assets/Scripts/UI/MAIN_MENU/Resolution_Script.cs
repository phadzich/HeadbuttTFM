using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Resolution_Script : MonoBehaviour
{
    public static Resolution_Script Instance;

    public TMP_Dropdown resolutionDropdown;

    private List<Vector2Int> availableResolutions = new List<Vector2Int>()
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440),
        new Vector2Int(1600, 1200),
        new Vector2Int(1280, 720),
        new Vector2Int(800, 600),
        new Vector2Int(640, 480)
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Lo llevo entre escenas
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

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var res in availableResolutions)
        {
            options.Add($"{res.x} x {res.y}");
        }
        resolutionDropdown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        ChangeResolution(savedIndex); // Aplico resolución guardada
    }

    void ChangeResolution(int index)
    {
        if (index < 0 || index >= availableResolutions.Count)
            return;

        Vector2Int selectedResolution = availableResolutions[index];
        Screen.SetResolution(selectedResolution.x, selectedResolution.y, FullScreenMode.Windowed);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        Debug.Log($"Resolución cambiada a: {selectedResolution.x}x{selectedResolution.y}");
    }
}
