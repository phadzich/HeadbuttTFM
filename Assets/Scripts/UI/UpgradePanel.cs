using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{

    [Header("UI")]
    public GameObject upgradeCardUIPrefab;
    public Transform upgradeCardsContainer;
    public GameObject pagesButtons;
    public GameObject EmptyListText;
    public int itemsPerPage = 2;

    private int currentPage = 0;

    private List<HelmetInstance> helmetsToUpgrade => HelmetManager.Instance.helmetsOwned;

    private void OnEnable()
    {

        UpdateList();
        CraftingManager.Instance.HelmetEvolved += UpdateList;
    }

    private void OnDisable()
    {
        CraftingManager.Instance.HelmetEvolved -= UpdateList;
    }

    private void UpdateList()
    {
        currentPage = 0;

        if (helmetsToUpgrade.Count == 0)
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
        foreach (Transform child in upgradeCardsContainer)
        {
            Destroy(child.gameObject);
        }

        int startIndex = currentPage * itemsPerPage;

        for (int i = 0; i < itemsPerPage; i++)
        {
            int index = startIndex + i;
            if (index >= helmetsToUpgrade.Count) break;

            HelmetInstance helmet = helmetsToUpgrade[index];
            Instantiate(upgradeCardUIPrefab, upgradeCardsContainer).GetComponent<UpgradeStatsCard>().SetUp(helmet);
        }
    }

    private int TotalPages()
    {
        return Mathf.CeilToInt((float)helmetsToUpgrade.Count / itemsPerPage);
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
