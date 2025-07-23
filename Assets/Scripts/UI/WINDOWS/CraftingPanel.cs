using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject helmetUIPrefab;
    public GameObject helmetInfoCardPrefab;
    public GameObject helmetListContainer;
    public GameObject cardContainer;
    public GameObject EmptyListText;

    private List<HelmetInstance> availableHelmets => HelmetManager.Instance.allHelmets;

    private void OnEnable()
    {

        LoadMainPage();
        CraftingManager.Instance.HelmetSelected += UpdateInfoCard;
        CraftingManager.Instance.HelmetEvolved += UpdateInfoCard;
        CraftingManager.Instance.HelmetEvolved += UpdateHelmetList;

    }

    private void OnDisable()
    {
        CraftingManager.Instance.HelmetSelected -= UpdateInfoCard;
        CraftingManager.Instance.HelmetEvolved -= UpdateInfoCard;
        CraftingManager.Instance.HelmetEvolved -= UpdateHelmetList;
        CraftingManager.Instance.SelectHelmet(null);
    }

    private void LoadMainPage()
    {
        UpdateHelmetList();
    }

    /* Funciones del panel de HELMETS */

    private void UpdateHelmetList()
    {
        // Borra los hijos actuales
        foreach (Transform child in helmetListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var _helmet in availableHelmets)
        {
            Instantiate(helmetUIPrefab, helmetListContainer.transform).GetComponent<HelmetCard>().SetUp(_helmet);
        }
    }


    private void UpdateInfoCard()
    {

        // Borra los hijos actuales
        foreach (Transform child in cardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        Instantiate(helmetInfoCardPrefab, cardContainer.transform).GetComponent<HelmetInfoCard>().SetUp(CraftingManager.Instance.selectedHelmet);
    }
}
