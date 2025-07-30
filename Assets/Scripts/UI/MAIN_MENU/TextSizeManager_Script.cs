using UnityEngine;
using TMPro;

public class TextSizeManager : MonoBehaviour
{
    public static TextSizeManager Instance;

    public TMP_Dropdown textSizeDropdown;
    public TMP_Text[] targetTexts;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Lo guardo entre escenas
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

        int savedIndex = PlayerPrefs.GetInt("TextSizeIndex", 0);
        textSizeDropdown.value = savedIndex;
        textSizeDropdown.RefreshShownValue();

        textSizeDropdown.onValueChanged.AddListener(ChangeTextSize);
        ChangeTextSize(savedIndex); // Aplico tamaño guardado
    }

    void ChangeTextSize(int index)
    {
        float size = 40f;

        switch (index)
        {
            case 0: size = 40f; break; // Normal
            case 1: size = 50f; break; // Big
            default: size = 40f; break;
        }

        foreach (TMP_Text text in targetTexts)
        {
            if (text != null)
                text.fontSize = size;
        }

        PlayerPrefs.SetInt("TextSizeIndex", index);
    }
}

