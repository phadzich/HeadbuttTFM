using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetCard : MonoBehaviour
{
    public TextMeshProUGUI helmetNameText;
    public Image helmetIcon;
    public TextMeshProUGUI lvlTxt;
    private HelmetInstance helmet;


    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetInstance helmetI)
    {
        helmet = helmetI;
        helmetNameText.text = helmetI.currentInfo.name;
        helmetIcon.sprite = helmetI.currentInfo.icon;
        lvlTxt.text = "Evolucion " + helmetI.helmetXP.currentEvolution;
    }

    // Cuando el jugador da click en Craft, se desbloquea el casco y los recursos se gastan, la lista se actualiza por medio del evento onOwnedResourcesChanged
    public void OnClickUpgradeBtn()
    {
        CraftingManager.Instance.SelectHelmet(helmet);
    }


}
