using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemsPanelUI : MonoBehaviour
{

    public Transform slotsCointainer;
    public EquippedItemButton equippedSlotPrefab;
    public void PanelStart(List<(Item item, int amount)> _equippedItems)
    {
        UpdateInfo(_equippedItems);
        EnableButtons(false);
    }

    public void UpdateInfo(List<(Item item, int amount)> _equippedItems)
    {
        ClearSlots();
        RefreshSlots(_equippedItems);
    }

    private void ClearSlots()
    {
        foreach (Transform _child in slotsCointainer)
        {
            Destroy(_child.gameObject);
        }
    }

    private void RefreshSlots(List<(Item item, int amount)> _equippedItems)
    {
        foreach (var _kvp in _equippedItems)
        {
            var item = _kvp.item;
            var count = _kvp.amount;

            Instantiate(equippedSlotPrefab, slotsCointainer).Setup(item, count);
        }
    }

    public void EnableButtons(bool _enabled)
    {
        foreach (Transform _item in slotsCointainer)
        {
            _item.GetComponent<Button>().interactable = _enabled;
        }
    }

}
