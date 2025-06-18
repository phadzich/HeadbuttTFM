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
    public GameObject blueprintUIPrefab;
    public GameObject helmetListContainer;
    public GameObject blueprintListContainer;
    public GameObject pagesButtons;
    public GameObject EmptyListText;

    public int itemsPerPage = 3;

    private int currentPage = 0;

    private List<HelmetInstance> availableHelmets => HelmetManager.Instance.GetHelmetsReadyToEvolve();

    private void OnEnable()
    {

        LoadMainPage();
        CraftingManager.Instance.HelmetSelected += ShowBlueprintPanel;
        CraftingManager.Instance.HelmetEvolved += LoadMainPage;

    }

    private void OnDisable()
    {
        CraftingManager.Instance.HelmetSelected -= ShowBlueprintPanel;
        CraftingManager.Instance.HelmetEvolved -= LoadMainPage;
    }

    private void LoadMainPage()
    {
        helmetListContainer.SetActive(true);
        blueprintListContainer.SetActive(false);
        UpdateHelmetList();
    }

    /* Funciones del panel de HELMETS */

    private void UpdateHelmetList()
    {
        currentPage = 0;

        if (availableHelmets.Count == 0)
        {
            EmptyListText.SetActive(true);
        }
        else
        {
            EmptyListText.SetActive(false);

            if (TotalPages() > 1)
            {
                pagesButtons.SetActive(true);
            }
            else
            {
                pagesButtons.SetActive(false);
            }
        }

        UpdatePage();
    }

    private void UpdatePage()
    {
        // Borra los hijos actuales
        foreach (Transform child in helmetListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        int startIndex = currentPage * itemsPerPage;

        for (int i = 0; i < itemsPerPage; i++)
        {
            int index = startIndex + i;
            if (index >= availableHelmets.Count) break;

            HelmetInstance helmet = availableHelmets[index];
            Instantiate(helmetUIPrefab, helmetListContainer.transform).GetComponent<HelmetCard>().SetUp(helmet);
        }
    }

    private int TotalPages()
    {
        return Mathf.CeilToInt((float)availableHelmets.Count / itemsPerPage);
    }

    public void NextPage()
    {
        currentPage++;
        if (currentPage >= TotalPages()) currentPage = 0; // ciclo
        UpdatePage();
    }

    public void PreviousPage()
    {
        currentPage--;
        if (currentPage < 0) currentPage = TotalPages() - 1; // ciclo
        UpdatePage();
    }

    /* Funciones del panel de BLUEPRINTS */

    public void ShowBlueprintPanel()
    {
        helmetListContainer.SetActive(false);
        blueprintListContainer.SetActive(true);
        pagesButtons.SetActive(false);
        UpdateBPList();
    }

    private void UpdateBPList()
    {
        List<HelmetBlueprint> blueprints = CraftingManager.Instance.GetUnlockedBlueprintsByEvolutionReq(CraftingManager.Instance.selectedHelmet.currentEvolution);
        // Borra los hijos actuales
        foreach (Transform child in blueprintListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var bp in blueprints)
        {
            Instantiate(blueprintUIPrefab, blueprintListContainer.transform).GetComponent<HelmetBluprintUI>().SetUp(bp);
        }
    }
}
