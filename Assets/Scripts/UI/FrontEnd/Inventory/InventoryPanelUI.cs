using System.Collections.Generic;
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
    public GameObject prompt;

    public Item currentSelectedItem;

    public void Init()
    {
        ownedItems = InventoryManager.Instance.itemsInventory.ownedItems;
        equippedItems = InventoryManager.Instance.itemsInventory.equippedItems;
    }

    private void OnEnable()
    {
        prompt.SetActive(true);
        InventoryManager.Instance.itemsInventory.ItemEquipped += OnItemEquipped;
        InventoryManager.Instance.itemsInventory.ItemOwned += OnItemOwned;
        InventoryManager.Instance.itemsInventory.ItemsListChanged += OnItemsListChanged;
        itemInfoPanel.PanelStart();
        ownedPanel.PanelStart(ownedItems);
        equippedPanel.PanelStart(equippedItems);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.itemsInventory.ItemEquipped -= OnItemEquipped;
        InventoryManager.Instance.itemsInventory.ItemOwned -= OnItemOwned;
        InventoryManager.Instance.itemsInventory.ItemsListChanged -= OnItemsListChanged;
    }

    private void OnItemEquipped(Item _item, int _amount)
    {
        itemInfoPanel.UpdateInfo(currentSelectedItem);
        equippedPanel.UpdateInfo(equippedItems);
    }
    private void OnItemOwned(Item _item, int _amount)
    {
        ownedPanel.UpdateInfo(ownedItems);
        equippedPanel.UpdateInfo(equippedItems);
    }
    public void OnItemsListChanged()
    {
        ownedPanel.UpdateInfo(ownedItems);
        equippedPanel.UpdateInfo(equippedItems);
        itemInfoPanel.UpdateInfo(currentSelectedItem);
    }

    public void ItemSelected(Item _item)
    {
        currentSelectedItem = _item;
        itemInfoPanel.UpdateInfo(currentSelectedItem);
    }


    public void EquipButtonClick()
    {
        PlayEquipSound();

        InventoryManager.Instance.itemsInventory.TryEquipItem(currentSelectedItem);
    }

    private void PlayEquipSound()
    {
        switch (currentSelectedItem.type)
        {
            case PotionTypes.Durability:
                SoundManager.PlaySound(UIType.EQUIP_HP);
                break;
            case PotionTypes.HBPoints:
                SoundManager.PlaySound(UIType.EQUIP_HB);
                break;
        }
    }

    public void ToggleSwapPanel(bool _show)
    {
        //Debug.Log($"SWAP MODE {_show}");
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
