using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject helmetButtonPrefab;
    public GameObject helmetListContainer;
    public TextMeshProUGUI helmetNameTXT;
    public Image helmetIcon;

    public Button craftBTN;
    public Button equipBTN;

    private List<HelmetInstance> availableHelmets => HelmetManager.Instance.allHelmets;

    private void OnEnable()
    {

        LoadMainPage();
        CraftingManager.Instance.HelmetSelected += UpdateInfoCard;
        //CraftingManager.Instance.HelmetEvolved += UpdateInfoCard;
        CraftingManager.Instance.HelmetEvolved += UpdateHelmetList;

    }

    private void OnDisable()
    {
        CraftingManager.Instance.HelmetSelected -= UpdateInfoCard;
        //CraftingManager.Instance.HelmetEvolved -= UpdateInfoCard;
        CraftingManager.Instance.HelmetEvolved -= UpdateHelmetList;
        CraftingManager.Instance.SelectHelmet(null);
    }

    private void LoadMainPage()
    {
        UpdateHelmetList();
    }

    /* Funciones del panel de HELMETS */

    public void UpdateHelmetList()
    {
        // Borra los hijos actuales
        foreach (Transform child in helmetListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var _helmet in availableHelmets)
        {
            Instantiate(helmetButtonPrefab, helmetListContainer.transform).GetComponent<HelmetCard>().SetUp(_helmet);
        }
    }


    public void UpdateInfoCard(HelmetInstance _helmetInstance)
    {
        helmetNameTXT.text = _helmetInstance.baseHelmet.name;
        helmetIcon.sprite = _helmetInstance.baseHelmet.icon;

        if (_helmetInstance.isDiscovered) {
            craftBTN.interactable = true;
            if (_helmetInstance.isCrafted)
            {
                equipBTN.interactable = true;
            }
            else
            {
                equipBTN.interactable = false;
            }
        }
        else
        {
            craftBTN.interactable = false;
            equipBTN.interactable = false;
        }
    }

}
