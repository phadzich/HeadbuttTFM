using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetUpgradeCard : MonoBehaviour
{
    public TextMeshProUGUI helmetNameText;
    public Image helmetIcon;
    public TextMeshProUGUI lvlTxt;
    public Transform resourceListContainer; // Donde se van a poner los ResourceIndicators
    public GameObject resourceIndicatorPrefab;

    private HelmetInstance helmet;


    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetInstance helmetI)
    {
        helmet = helmetI;
        helmetNameText.text = helmetI.baseHelmet.helmetName;
        helmetIcon.sprite = helmetI.baseHelmet.icon;
        lvlTxt.text = "LVL " + helmetI.helmetXP.currentLevel + " -> " + "LVL " + helmetI.helmetXP.nextLevel;

        SetResources(helmet.GetPriceForNextLevel());
    }

    // Crea los prefabs que muestran la cantidad de recursos
    private void SetResources(List<ResourceRequirement> resources)
    {
        // Instancia uno por cada blueprint
        foreach (var resource in resources)
        {
            GameObject res = Instantiate(resourceIndicatorPrefab, resourceListContainer);
            ResourceIndicator resourceUI = res.GetComponent<ResourceIndicator>();
            resourceUI.SetupIndicator(resource.resource, resource.quantity);
        }
    }

    // Cuando el jugador da click en Craft, se desbloquea el casco y los recursos se gastan, la lista se actualiza por medio del evento onOwnedResourcesChanged
    public void OnClickUpgradeBtn()
    {
        CraftingManager.Instance.UpgradeHelmet(helmet);
    }


}
