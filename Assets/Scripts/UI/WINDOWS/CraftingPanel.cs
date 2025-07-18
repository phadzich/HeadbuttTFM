using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject helmetBlueprintPrefab;
    public GameObject helmetListContainer;
    public GameObject EmptyListText;

    private HashSet<HelmetBlueprint> availableHelmets => CraftingManager.Instance.unlockedBlueprints;

    private void OnEnable()
    {

        LoadMainPage();

    }

    private void LoadMainPage()
    {
        UpdateHelmetBPList();
    }

    /* Funciones del panel de HELMETS */

    private void UpdateHelmetBPList()
    {
        // Borra los hijos actuales
        foreach (Transform child in helmetListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var _blueprint in availableHelmets)
        {
            Instantiate(helmetBlueprintPrefab, helmetListContainer.transform).GetComponent<HelmetBluprintUI>().SetUp(_blueprint);
        }
    }

}
