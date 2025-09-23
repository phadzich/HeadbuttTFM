using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnedItemsPanelUI : MonoBehaviour
{

    public Transform slotsCointainer;
    public ItemButton slotPrefab;

    public void PanelStart(Dictionary<Item, int> _ownedItems)
    {
        UpdateInfo(_ownedItems);
    }

    public void UpdateInfo(Dictionary<Item, int> _ownedItems)
    {
        ClearSlots();
        RefreshSlots(_ownedItems);
    }

    private void ClearSlots()
    {
        foreach (Transform _child in slotsCointainer)
        {
            Destroy(_child.gameObject);
        }
    }

    private void RefreshSlots(Dictionary<Item, int> _ownedItems)
    {
        
        foreach (var _kvp in _ownedItems)
        {
            var item = _kvp.Key;
            var count = _kvp.Value;

            Instantiate(slotPrefab, slotsCointainer).Setup(item, count);
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
