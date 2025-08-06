using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TextSizeManager : MonoBehaviour
{
    public static TextSizeManager Instance;

    public TMP_Dropdown textSizeDropdown;
    public TMP_Text[] targetTexts;

    private Dictionary<TMP_Text, float> originalSizes = new Dictionary<TMP_Text, float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Base para cambio de escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (textSizeDropdown == null)
        {
            Debug.LogWarning("Dropdown de tamaño de texto no asignado.");
            return;
        }

        // Aqui guardo los tamaños originales para cambiar entre normal y big y viceversa
        foreach (TMP_Text text in targetTexts)
        {
            if (text != null && !originalSizes.ContainsKey(text))
            {
                originalSizes[text] = text.fontSize;
            }
        }

        int savedIndex = PlayerPrefs.GetInt("TextSizeIndex", 0);
        textSizeDropdown.value = savedIndex;
        textSizeDropdown.RefreshShownValue();

        textSizeDropdown.onValueChanged.AddListener(ChangeTextSize);
        ChangeTextSize(savedIndex); // Aplico el tamaño guardado
    }

    void ChangeTextSize(int index)
    {
        foreach (TMP_Text text in targetTexts)
        {
            if (text == null || !originalSizes.ContainsKey(text)) continue;

            float baseSize = originalSizes[text];

            // Incrementa la escala segun la siguiente logica
            if (Mathf.Approximately(baseSize, 40f))
            {
                text.fontSize = index == 0 ? 40f : 50f;
            }
            else if (Mathf.Approximately(baseSize, 16f))
            {
                text.fontSize = index == 0 ? 16f : 22f;
            }
            else if (Mathf.Approximately(baseSize, 24f))
            {
                text.fontSize = index == 0 ? 24f : 32f;
            }
            else
            {
                Debug.Log($"Tamaño de texto no gestionado: {baseSize}, se mantiene.");
            }
        }

        PlayerPrefs.SetInt("TextSizeIndex", index);
    }
}


