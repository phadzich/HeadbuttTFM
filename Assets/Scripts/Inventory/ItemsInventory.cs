using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemsInventory : MonoBehaviour
{

    public Item currentActiveItem;
    public int currentActiveIndex;

    public int maxItemsEquipped;

    [SerializeField]
    public Dictionary<Item, int> ownedItems;
    [SerializeField]
    public List<(Item item, int amount)> equippedItems;

    public Action ItemsListChanged;
    public Action<Item, int> ItemOwned;
    public Action<Item, int> ItemEquipped;
    public Action<Item, int> ItemConsumed;
    public Action<Item, int> ItemCycled;

    public void Init()
    {
        ownedItems = new Dictionary<Item, int>();
        equippedItems = new List<(Item, int)>();
        UIManager.Instance.InventoryPanel.Init();
    }


    public void ChangeActiveItem()
    {
        if (equippedItems.Count > 0)
        {
            currentActiveItem = equippedItems[currentActiveIndex].item;
        }
    }
    public void TryAddOwnedItems(Item _item, int _amount)
    {

        if (ownedItems.ContainsKey(_item))
        {
            int _totalAmount = ownedItems[_item] + _amount;
            ownedItems[_item] = _totalAmount;
        }
        else
        {
            ownedItems.Add(_item, _amount);
        }
        ItemOwned?.Invoke(_item, ownedItems[_item]);
    }

    public void TryEquipItem(Item _item)
    {
        Debug.Log(_item.itemName);
        bool alreadyEquipped = equippedItems.Any(e => e.item == _item);

        if (!alreadyEquipped)
        {
            Debug.Log("ITEM NOT EQUIPPED");

            if (equippedItems.Count < maxItemsEquipped)
            {
                EquipNewItem(_item, ownedItems[_item]);
                ItemEquipped?.Invoke(_item, ownedItems[_item]);
                ItemsListChanged?.Invoke();
                ActivateNextItem();
            }
            else
            {
                UIManager.Instance.InventoryPanel.ToggleSwapPanel(true);
            }
        }
        else
        {
            Debug.Log("ITEM ALREADY EQUIPPED");
        }

    }

    public void SwapHelmet(Item _itemIn, Item _itemOut)
    {
//
    }

    private void EquipNewItem(Item _newItem, int _amount)
    {
        equippedItems.Add((_newItem, _amount));
    }
    public void UseActiveItem(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {

            if (ownedItems.Count <= 0)
            {
                return;
            }
            Debug.Log("!");

            ConsumeItems(currentActiveItem, 1);
        }
    }
    public void ConsumeItems(Item _item, int _amount)
    {
        int _prevAmount = ownedItems[_item];
        int _finalAmount = _prevAmount - _amount;

        if (_finalAmount == 0)
        {
            ownedItems.Remove(_item);
            equippedItems.RemoveAll(e => e.item == _item);
            ActivateNextItem();
        }
        else
        {
            ownedItems[_item] = _finalAmount;
            int index = equippedItems.FindIndex(e => e.item == _item);
            if (index >= 0)
            {
                equippedItems[index] = (_item, _finalAmount);
            }
            ItemConsumed?.Invoke(_item, _finalAmount);
        }

        _item.Use();
    }

    public void NextEquippedItem(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {

            if (equippedItems.Count <= 1) return;
            ActivateNextItem();
        }
    }

    public void ActivateNextItem()
    {

        if (equippedItems.Count <= 0)
        {
            ItemCycled?.Invoke(null, 0);
            return;
        }
        currentActiveIndex = (currentActiveIndex + 1) % equippedItems.Count;
        ChangeActiveItem();

        ItemCycled?.Invoke(currentActiveItem, equippedItems[currentActiveIndex].amount);
    }

    public void ActivatePrevItem()
    {
        if (equippedItems.Count <= 0)
        {
            ItemCycled?.Invoke(null, 0);
            return;
        }
        currentActiveIndex = (currentActiveIndex - 1 + equippedItems.Count) % equippedItems.Count;
        ChangeActiveItem();
        ItemCycled?.Invoke(currentActiveItem, equippedItems[currentActiveIndex].amount);
    }

    public void PreviousEquippedItem(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {

            if (equippedItems.Count <= 1) return;
            ActivatePrevItem();
        }
    }   
    }
