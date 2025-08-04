using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ColorBlindManager : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    public TMP_Dropdown typeDropdown;

    [Header("Texturas LUT para cada tipo de daltonismo")]
    public Texture2D protanopiaLUT;
    public Texture2D deuteranopiaLUT;
    public Texture2D tritanopiaLUT;

    private ColorLookup colorLookup;

    void Start()
    {
        
        if (typeDropdown == null)
        {
            GameObject dropdownGO = GameObject.Find("Color_Blind_Dropdown");
            if (dropdownGO != null)
                typeDropdown = dropdownGO.GetComponent<TMP_Dropdown>();
            else
                Debug.LogWarning("Dropdown 'Color_Blind_Dropdown' no encontrado en la escena.");
        }

        // Obtengo el componente ColorLookup del Volume Profile creado previamente
        if (volumeProfile != null && volumeProfile.TryGet(out colorLookup))
        {
            string savedType = PlayerPrefs.GetString("colorblind_type", "ninguno");

            int index = typeDropdown.options.FindIndex(o => o.text.ToLower().Trim() == savedType.Trim());

            if (index >= 0)
            {
                typeDropdown.value = index;
                typeDropdown.RefreshShownValue();
            }
            else
            {
                Debug.LogWarning($"⚠️ No se encontró opción en el Dropdown con el texto: '{savedType}'");
            }

            ApplyLUT(savedType);
        }


        // Cambio las opciones dentro del dropdown
        typeDropdown.onValueChanged.AddListener(delegate {
            string selected = typeDropdown.options[typeDropdown.value].text.ToLower();
            ApplyLUT(selected);
            PlayerPrefs.SetString("colorblind_type", selected);
        });
    }

    public void OnDropdownChanged(int index)
    {
        string selected = typeDropdown.options[index].text.ToLower();
        ApplyLUT(selected);
        PlayerPrefs.SetString("colorblind_type", selected);
    }

    void ApplyLUT(string type)
    {
        if (colorLookup == null) return;

        type = type.ToLower().Trim(); 

        Texture2D selectedLUT = null;

        switch (type)
        {
            case "protanopia": selectedLUT = protanopiaLUT; break;
            case "deuteranopia": selectedLUT = deuteranopiaLUT; break;
            case "tritanopia": selectedLUT = tritanopiaLUT; break;
            case "ninguno": colorLookup.active = false; return;
        }

        if (selectedLUT != null)
        {
            colorLookup.active = true;
            colorLookup.texture.value = selectedLUT;
        }
        else
        {
            Debug.LogWarning($"LUT no asignado para tipo '{type}'. Revisa los campos en el Inspector.");
            colorLookup.active = false;
        }
    }

}
