using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetInfoCard : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI helmetNameText;
    public TextMeshProUGUI helmetDescText;
    public Image helmetIcon;
    public Button evolveBtn;

    public GameObject resourcePrefab;
    public GameObject resourceContainer;

    private HelmetInstance helmet;

    [Header("Stat bars")]
    public StatBar durabilityStat;

    // Se crea el prefab con la información del helmet
    public void SetUp(HelmetInstance _helmetI)
    {
        helmet = _helmetI;
        helmetNameText.text = _helmetI.currentInfo.name;
        helmetDescText.text = _helmetI.currentInfo.description;
        helmetIcon.sprite = _helmetI.currentInfo.icon;

        durabilityStat.UpdateBar(_helmetI.currentDurability);
        durabilityStat.SetMaxVal(7);

        if (!CanHideUpgradeItems()) // Si aun se puede upgradear seguimos mostrando el boton y el costo
        {
            SetUpButton();
            SetUpResources();
        }
    }

    private bool CanHideUpgradeItems()
    {
        if (helmet.currentLevel == 3)
        {
            evolveBtn.gameObject.SetActive(false);
            resourceContainer.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    private void SetUpButton()
    {
        if (helmet.CanEvolve())
        {
            evolveBtn.interactable = true;
        }
        else
        {
            evolveBtn.interactable = false;
        }
    }

    private void SetUpResources()
    {
        // Borra los hijos actuales
        foreach (Transform child in resourceContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var _res in helmet.GetUpgradeRequirement(helmet.nextLevel).requirements)
        {
            Instantiate(resourcePrefab, resourceContainer.transform).GetComponent<ResourceIndicator>().SetupIndicator(_res.resource,_res.quantity);
        }
    }

    public void EvolveBtnOnClick()
    {
        CraftingManager.Instance.EvolveHelmet();
    }
}
