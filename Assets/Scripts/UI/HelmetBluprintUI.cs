using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetBluprintUI : MonoBehaviour
{

    public TextMeshProUGUI blueprintNameText;
    public TextMeshProUGUI blueprintDescriptionText;
    public Image blueprintIcon;
    public Button craftBtn;
    public Transform resourceListContainer; // Donde se van a poner los ResourceIndicators
    public GameObject resourceIndicatorPrefab;

    private HelmetBlueprint blueprint;

    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetBlueprint _blueprint)
    {
        blueprint = _blueprint;
        blueprintNameText.text = _blueprint.helmetInfo.name;
        blueprintDescriptionText.text = _blueprint.helmetInfo.description;
        blueprintIcon.sprite = _blueprint.helmetInfo.icon;

        CheckIfCanCraft(_blueprint);
        SetResources(_blueprint.requiredResources);
    }

    private void CheckIfCanCraft(HelmetBlueprint _blueprint)
    {
        if (_blueprint.CanCraft(ResourceManager.Instance.ownedResources))
        {
            craftBtn.interactable = true;
        }
        else
        {
            craftBtn.interactable = false;
        }
    }

    // Crea los prefabs que muestran la cantidad de recursos
    private void SetResources(List<ResourceRequirement> _resources)
    {
        // Instancia uno por cada blueprint
        foreach (var resource in _resources)
        {
            GameObject res = Instantiate(resourceIndicatorPrefab, resourceListContainer);
            ResourceIndicator resourceUI = res.GetComponent<ResourceIndicator>();
            resourceUI.SetupIndicator(resource.resource, resource.quantity);
        }
    }

    // Cuando el jugador da click en Craft, se desbloquea el casco y los recursos se gastan, la lista se actualiza por medio del evento onOwnedResourcesChanged
    public void OnClickCraftingBtn()
    {
        CraftingManager.Instance.EvolveHelmet(blueprint);
    }
}
