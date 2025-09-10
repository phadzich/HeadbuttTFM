using System.Collections.Generic;
using UnityEngine;

public class ShopItemsPanelUI : MonoBehaviour
{

    public Transform slotsCointainer;
    public ShopItemButton slotPrefab;

    public void PanelStart(List<ShopItem> _shopItems)
    {
        UpdateInfo(_shopItems);
    }

    public void UpdateInfo(List<ShopItem> _shopItems)
    {
        ClearSlots();
        RefreshSlots(_shopItems);
    }

    private void ClearSlots()
    {
        foreach (Transform _child in slotsCointainer)
        {
            Destroy(_child.gameObject);
        }
    }

    private void RefreshSlots(List<ShopItem> _shopItems)
    {
        foreach (ShopItem _item in _shopItems)
        {
            Instantiate(slotPrefab, slotsCointainer).Setup(_item);
        }
    }


}
