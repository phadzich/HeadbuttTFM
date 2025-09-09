using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public ItemsInventory itemsInventory;

    [SerializeField]private InventorySlot highlightedSlot;
    [SerializeField] private InventorySlot selectedSlot;
    [SerializeField] private InventorySlot activeSlot;

    public void UpdateEquippedSlotData(Item _item, int _count, InventorySlot _slot)
    {
        Debug.Log($"UDATINGSLOT {_slot}");
        _slot.EquipItem(_item, _count);
    }

    public void HighlightSlot(InventorySlot _slot)
    {
        if (highlightedSlot != null)
        {
            highlightedSlot.SetHighlighted(false);
        }

        highlightedSlot = _slot;
        highlightedSlot.SetHighlighted(true);
    }

    public void UnhighlightSlot(InventorySlot _slot)
    {
        highlightedSlot.SetHighlighted(false);
        highlightedSlot = null;
    }

    public void SelectSlot(InventorySlot _slot)
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetSelected(false);
        }
        if (_slot.slotType == SlotType.Item || _slot.slotType == SlotType.Helmet)
        {
            activeSlot = _slot;
            Debug.Log($"USING {_slot.itemCount}x{_slot.itemData.itemName}");
        }

        if (_slot.slotType == SlotType.EquippedItem)
        {
            Debug.Log($"TRYING EQUIP {activeSlot.itemCount}x{activeSlot.itemData.itemName}");
            itemsInventory.TryEquipItem(activeSlot.itemData, activeSlot.itemCount,_slot);
        }

        selectedSlot = _slot;
        selectedSlot.SetSelected(true);

        UIManager.Instance.NPCInventoryPanel.ContextFromSelectedSlot(_slot);
    }

    public void ClearSelection()
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetSelected(false);
            selectedSlot = null;
        }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("InventoryManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        itemsInventory.Init();
    }


}

public enum NavContext { ItemSlots, EquippedItems }
