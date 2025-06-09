using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject helmetUIPrefab;
    public GameObject blueprintUIPrefab;
    public Transform helmetListContainer;
    public Transform blueprintListContainer;
    public GameObject pagesButtons;
    public GameObject EmptyListText;

    public int itemsPerPage = 3;

    private int currentPage = 0;

    private List<HelmetInstance> availableHelmets => HelmetManager.Instance.GetHelmetsReadyToEvolve();

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
        foreach (Transform child in helmetListContainer)
        {
            Destroy(child.gameObject);
        }

        int startIndex = currentPage * itemsPerPage;

        for (int i = 0; i < itemsPerPage; i++)
        {
            int index = startIndex + i;
            if (index >= availableHelmets.Count) break;

            HelmetInstance helmet = availableHelmets[index];
            Instantiate(helmetUIPrefab, helmetListContainer).GetComponent<HelmetUpgradeCard>().SetUp(helmet);
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
}
