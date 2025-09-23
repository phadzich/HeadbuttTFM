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
    public GameObject cancelButton;
    public GameObject swapBorder;
    public GameObject selectPrompt;
    public SwapHelmetsPanelUI swapHelmetsUI;

    private List<HelmetInstance> availableHelmets => HelmetManager.Instance.allHelmets;

    private void OnEnable()
    {
        selectPrompt.SetActive(true);
        LoadMainPage();
        CraftingManager.Instance.HelmetSelected += infoPanel.UpdateInfoCard;
        CraftingManager.Instance.HelmetCrafted += UpdateHelmetList;
        infoPanel.gameObject.SetActive(false);
        infoPanel.helmetIcon.gameObject.SetActive(false);
        infoPanel.equippedLabel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CraftingManager.Instance.HelmetSelected -= infoPanel.UpdateInfoCard;
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
            Instantiate(helmetButtonPrefab, helmetListContainer.transform).GetComponent<HelmetItemButton>().SetUp(_helmet);
        }

        swapHelmetsUI.UpdateHelmetList();

    }

    public void ToggleSwapPanel(bool _show)
    {
        Debug.Log($"SWAP MODE {_show}");
        SwapMode(_show);
        infoPanel.UpdateData();
    }
        

    private void SwapMode(bool _value)
    {
        cancelButton.SetActive(_value);
        swapBorder.SetActive(_value);
        foreach (Transform _button in swapHelmetsUI.helmetListContainer.transform)
        {
            _button.GetComponent<Button>().interactable = _value;
        }
        foreach (Transform _helmet in helmetListContainer.transform)
        {
            _helmet.GetComponent<Button>().interactable = !_value;
        }
    }
}
