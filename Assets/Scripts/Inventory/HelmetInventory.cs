using System.Collections.Generic;
using UnityEngine;

public class HelmetInventory : MonoBehaviour
{

    [SerializeField]
    private List<HelmetInstance> currentCraftedHelmets = new List<HelmetInstance>();
    private List<HelmetInstance> currentEquippedHelmets = new List<HelmetInstance>();
    public void Init()
    {
        UpdateHelmetsInventory();
    }

    public void UpdateHelmetsInventory()
    {
        GetCraftedHelmets();
        GetEquippedHelmets();
    }

private void GetCraftedHelmets()
    {
        currentCraftedHelmets.Clear();
        foreach (HelmetInstance _instance in HelmetManager.Instance.allHelmets)
        {
            if (_instance.isCrafted)
            {
                currentCraftedHelmets.Add(_instance);
            }
        }
    }

    private void GetEquippedHelmets()
    {
        currentEquippedHelmets.Clear();
        currentCraftedHelmets = HelmetManager.Instance.helmetsEquipped;
    }
}
