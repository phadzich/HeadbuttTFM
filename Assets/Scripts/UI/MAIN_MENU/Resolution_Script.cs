using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Resolution_Script : MonoBehaviour
{
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

    void Start()
    {
        // Aqui debo asignar el dropdown
        if (resolutionDropdown == null)
        {
            Debug.LogError("El TMP_Dropdown no está asignado en el Inspector.");
            return;
        }

        // Aqui debo seleccionar las resoluciones que tengo
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var res in availableResolutions)
        {
            options.Add($"{res.x} x {res.y}");
        }
        resolutionDropdown.AddOptions(options);

       
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        
        ChangeResolution(resolutionDropdown.value);
    }

    void ChangeResolution(int index)
    {
        Vector2Int selectedResolution = availableResolutions[index];
        Screen.SetResolution(selectedResolution.x, selectedResolution.y, FullScreenMode.Windowed);
        Debug.Log($"Resolución cambiada a: {selectedResolution.x}x{selectedResolution.y}");
    }
}
