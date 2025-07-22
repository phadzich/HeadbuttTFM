using UnityEngine;
using TMPro;

public class TextSizeManager : MonoBehaviour
{
    public TMP_Dropdown textSizeDropdown;
    public TMP_Text[] targetTexts;

    void Start()
    {
        textSizeDropdown.onValueChanged.AddListener(ChangeTextSize);
        ChangeTextSize(textSizeDropdown.value); // Aca aplico el tamaño inicial
    }

    void ChangeTextSize(int index)
    {
        float size = 40f; // Valor por defecto

        switch (index)
        {
            case 0: // Normal
                size = 40f;
                break;
            case 1: // Big
                size = 50f;
                break;
        }

        foreach (TMP_Text text in targetTexts)
        {
            if (text != null)
                text.fontSize = size;
        }
    }
}


