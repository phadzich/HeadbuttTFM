using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ColorBlindManager : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    public Volume volume; // Aqui asigno el mismo volume profile para controlar el nivel de weight del modo daltonico
    public TMP_Dropdown typeDropdown;
    public Slider contributionSlider;

    [Header("Texturas LUT para cada tipo de daltonismo")]
    public Texture2D protanopiaLUT;
    public Texture2D deuteranopiaLUT;
    public Texture2D tritanopiaLUT;

    private ColorLookup colorLookup;

    private const string ContributionKey = "ColorblindContribution";
    private const string TypeKey = "colorblind_type";

    void Start()
    {
        
        if (typeDropdown == null)
        {
            GameObject dropdownGO = GameObject.Find("Color_Blind_Dropdown");
            if (dropdownGO != null)
                typeDropdown = dropdownGO.GetComponent<TMP_Dropdown>();
            else
                Debug.LogWarning("Dropdown 'Color_Blind_Dropdown' no encontrado.");
        }

        // Busco el colorlookup
        if (volumeProfile != null && volumeProfile.TryGet(out colorLookup))
        {
            string savedType = PlayerPrefs.GetString(TypeKey, "ninguno");
            int index = typeDropdown.options.FindIndex(o => o.text.ToLower().Trim() == savedType.Trim());

            if (index >= 0)
            {
                typeDropdown.value = index;
                typeDropdown.RefreshShownValue();
            }

            ApplyLUT(savedType);
        }

        // Cargo valor de Contribution guardado
        float savedContribution = PlayerPrefs.GetFloat(ContributionKey, 1f);
        if (volume != null)
        {
            volume.weight = savedContribution;
        }
        if (contributionSlider != null)
        {
            contributionSlider.value = savedContribution;
            contributionSlider.onValueChanged.AddListener(SetContribution);
        }

        
        typeDropdown.onValueChanged.AddListener(delegate {
            string selected = typeDropdown.options[typeDropdown.value].text.ToLower();
            ApplyLUT(selected);
            PlayerPrefs.SetString(TypeKey, selected);
        });
    }

    public void SetContribution(float value)
    {
        if (volume != null)
        {
            volume.weight = value;
            PlayerPrefs.SetFloat(ContributionKey, value);
        }
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
            case "ninguno":
                colorLookup.active = false;
                return;
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

