using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetItemButton : MonoBehaviour
{
    public Image helmetIcon;
    private HelmetInstance helmet;
    public Color secretColor;
    public GameObject newIndicator;
    public GameObject eqpIndicator;

    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetInstance helmetI)
    {
        helmet = helmetI;
        helmetIcon.sprite = helmetI.baseHelmet.icon;

        if (helmetI.isDiscovered)
        {
            helmetIcon.color = Color.white;
            if (!helmetI.isCrafted)
            {
                newIndicator.SetActive(true);
            }
        }
        else
        {
            newIndicator.SetActive(false);
            this.gameObject.GetComponent<Button>().interactable = false;
            helmetIcon.color = secretColor;
        }
        if (helmetI.isEquipped)
        {
            eqpIndicator.SetActive(true);
        } else {
            eqpIndicator.SetActive(false);
        }

    }

    // Cuando el jugador da click en Craft, se desbloquea el casco y los recursos se gastan, la lista se actualiza por medio del evento onOwnedResourcesChanged
    public void OnClickSelectBtn()
    {
        CraftingManager.Instance.SelectHelmet(helmet);
        UIManager.Instance.craftingPanel.selectPrompt.SetActive(false);
    }


}
