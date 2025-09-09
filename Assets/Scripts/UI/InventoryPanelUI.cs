using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanelUI : MonoBehaviour
{
    public OwnedItemsPanelUI ownedPanel;
    public EquippedItemsPanelUI equippedPanel;
    public ItemInfoPanelUI itemInfoPanel;

    public Dictionary<Item, int> ownedItems;
    public List<(Item item, int amount)> equippedItems;

    public GameObject cancelButton;
    public GameObject swapBorder;

    public Item currentSelectedItem;

    public void Init()
    {
        ownedItems = InventoryManager.Instance.itemsInventory.ownedItems;
        equippedItems = InventoryManager.Instance.itemsInventory.equippedItems;
    }

    private void OnEnable()
    {
        
        InventoryManager.Instance.itemsInventory.ItemEquipped += OnItemEquipped;
        InventoryManager.Instance.itemsInventory.ItemOwned += OnItemOwned;
        InventoryManager.Instance.itemsInventory.ItemsListChanged += OnItemsListChanged;
        //infoPanel.PanelStart();
        ownedPanel.PanelStart(ownedItems);
        equippedPanel.PanelStart(equippedItems);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.itemsInventory.ItemEquipped -= OnItemEquipped;
        InventoryManager.Instance.itemsInventory.ItemOwned -= OnItemOwned;
        InventoryManager.Instance.itemsInventory.ItemsListChanged -= OnItemsListChanged;
        //infoPanel.gameObject.SetActive(false);
    }

    private void OnItemEquipped(Item _item, int _amount)
    {
        //infoPanel.UpdateInfo();
        equippedPanel.UpdateInfo(equippedItems);
    }
    private void OnItemOwned(Item _item, int _amount)
    {
        ownedPanel.UpdateInfo(ownedItems);
        equippedPanel.UpdateInfo(equippedItems);
    }
    private void OnItemsListChanged()
    {
        ownedPanel.UpdateInfo(ownedItems);
        equippedPanel.UpdateInfo(equippedItems);
    }

    public void ItemSelected(Item _item)
    {
        currentSelectedItem = _item;
        itemInfoPanel.UpdateInfo(currentSelectedItem);
    }


    public void EquipButtonClick()
    {
        InventoryManager.Instance.itemsInventory.TryEquipItem(currentSelectedItem);
    }
    public void ToggleSwapPanel(bool _show)
    {
        Debug.Log($"SWAP MODE {_show}");
        SwapMode(_show);
        itemInfoPanel.UpdateData();
    }


    private void SwapMode(bool _value)
    {
        cancelButton.SetActive(_value);
        swapBorder.SetActive(_value);
        equippedPanel.EnableButtons(_value);
        ownedPanel.EnableButtons(!_value);

    }

}
