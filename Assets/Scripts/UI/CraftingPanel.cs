using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject blueprintUIPrefab;
    public Transform blueprintListContainer;
    public GameObject pagesButtons;
    public GameObject EmptyListText;
    public int itemsPerPage = 3;

    private int currentPage = 0;

    [Header("Prices")]
    public int jumpQuantity = 3;
    public int headbuttQuantity = 2;

    private List<HelmetBlueprint> availableBlueprints = new();

    private void OnEnable()
    {

        UpdateList();

        ResourceManager.Instance.onOwnedResourcesChanged += UpdateList;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged -= UpdateList;
    }

    private void UpdateList()
    {
        currentPage = 0;

        if (availableBlueprints.Count == 0)
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
        foreach (Transform child in blueprintListContainer)
        {
            Destroy(child.gameObject);
        }

        int startIndex = currentPage * itemsPerPage;

        for (int i = 0; i < itemsPerPage; i++)
        {
            int index = startIndex + i;
            if (index >= availableBlueprints.Count) break;

            HelmetBlueprint blueprint = availableBlueprints[index];
            Instantiate(blueprintUIPrefab, blueprintListContainer).GetComponent<HelmetBluprintUI>().SetUp(blueprint);
        }
    }

    private int TotalPages()
    {
        return Mathf.CeilToInt((float)availableBlueprints.Count / itemsPerPage);
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
}
