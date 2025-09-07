using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI")]
    public HelmetInfoPanelUI infoPanel;
    public GameObject helmetButtonPrefab;
    public GameObject helmetListContainer;

    public SwapHelmetsPanelUI swapHelmetsUI;

    private List<HelmetInstance> availableHelmets => HelmetManager.Instance.allHelmets;

    private void OnEnable()
    {

        LoadMainPage();
        CraftingManager.Instance.HelmetSelected += infoPanel.UpdateInfoCard;
        //CraftingManager.Instance.HelmetEvolved += UpdateInfoCard;
        CraftingManager.Instance.HelmetCrafted += UpdateHelmetList;
        infoPanel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CraftingManager.Instance.HelmetSelected -= infoPanel.UpdateInfoCard;
        //CraftingManager.Instance.HelmetEvolved -= UpdateInfoCard;
        CraftingManager.Instance.HelmetCrafted -= UpdateHelmetList;
        CraftingManager.Instance.SelectHelmet(null);
        infoPanel.gameObject.SetActive(false);
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

    public void ToggleSwapPanel(bool _show)
    {
        swapHelmetsUI.gameObject.SetActive(_show);
    }
}
