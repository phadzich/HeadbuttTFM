using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetItemButton : MonoBehaviour
{
    public Image helmetIcon;
    private HelmetInstance helmet;
    public Color secretColor;


    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetInstance helmetI)
    {
        helmet = helmetI;
        helmetIcon.sprite = helmetI.baseHelmet.icon;

        if (helmetI.isDiscovered)
        {
            helmetIcon.color = Color.white;
        }
        else
        {
            this.gameObject.GetComponent<Button>().interactable = false;
            helmetIcon.color = secretColor;
        }

    }

    // Cuando el jugador da click en Craft, se desbloquea el casco y los recursos se gastan, la lista se actualiza por medio del evento onOwnedResourcesChanged
    public void OnClickSelectBtn()
    {
        CraftingManager.Instance.SelectHelmet(helmet);
    }


}
