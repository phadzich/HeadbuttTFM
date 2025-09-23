using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class ColorBlindHandler : MonoBehaviour
{

    public Volume colorblindVolume; 


    [Header("Texturas LUT para cada tipo de daltonismo")]
    public Texture2D protanopiaLUT;
    public Texture2D deuteranopiaLUT;
    public Texture2D tritanopiaLUT;

    private ColorLookup colorLookup;

    void Start()
    {
        // Busco el colorlookup

        if (colorblindVolume != null && colorblindVolume.profile != null)
        {
            // Busca el override de ColorAdjustments en tu SettingsVolume
            if (!colorblindVolume.profile.TryGet(out colorLookup))
            {
                Debug.LogWarning("El ColorBlindVolume no tiene Color Lookup agregado.");
            }
        }

    }

    public void SetContribution(float value)
    {
        if (colorblindVolume != null)
        {
            colorblindVolume.weight = value;
        }
    }

    public void ApplyLUT(int mode)
    {
        if (colorLookup == null) return;

        Texture2D selectedLUT = null;

        switch (mode)
        {
            case 0: colorLookup.active = false; return;
            case 1: selectedLUT = deuteranopiaLUT; break;
            case 2: selectedLUT = tritanopiaLUT; break;
            case 3: selectedLUT = protanopiaLUT; break;

        }
        colorLookup.active = true;
        colorLookup.texture.value = selectedLUT;
    }
}

