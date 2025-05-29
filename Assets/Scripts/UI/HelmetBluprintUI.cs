using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetBluprintUI : MonoBehaviour
{

    public TextMeshProUGUI blueprintNameText;
    public Image blueprintIcon;
    public Transform resourceListContainer; // Donde se van a poner los ResourceIndicators
    public GameObject resourceIndicatorPrefab;

    private HelmetBlueprint helmet;

    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetBlueprint blueprint)
    {
        helmet = blueprint;
        blueprintNameText.text = blueprint.recipeName;
        blueprintIcon.sprite = blueprint.resultHelmet.icon;

        SetResources(blueprint.requiredResources);
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
    public void OnClickCraftingBtn()
    {
        List<ResourceRequirement> resources = helmet.requiredResources;

        foreach(var res in resources)
        {
            ResourceManager.Instance.SpendResource(res.resource, res.quantity);
        }

        HelmetManager.Instance.UnlockHelmet(helmet.resultHelmet);
    }
}
