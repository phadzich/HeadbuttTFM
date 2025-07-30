using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetSwapButton : MonoBehaviour
{
    public Image helmetIcon;
    private HelmetInstance helmet;


    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetInstance helmetI)
    {
        helmet = helmetI;
        helmetIcon.sprite = helmetI.baseHelmet.icon;
    }

   
    public void OnClickSelectBtn()
    {
        HelmetManager.Instance.SwapHelmet(CraftingManager.Instance.selectedHelmet,helmet);
        UIManager.Instance.craftingPanel.ToggleSwapPanel(false);
    }


}
