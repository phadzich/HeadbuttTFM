using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemsInventory : MonoBehaviour
{
    public Item currentActiveItem;
    public int currentActiveIndex;
    public List<Item> activeItemKeys;

    public Dictionary<Item, int> equippedItems;

    public Action<Item, int> ItemEquipped;
    public Action<Item, int> ItemConsumed;
    public Action<Item, int> ItemCycled;

    public void Init()
    {
        equippedItems = new Dictionary<Item, int>();
    }

    public void ChangeActiveItem()
    {
        currentActiveItem = activeItemKeys[currentActiveIndex];

    }
    public void TryEquipItems(Item _item, int _amount)
    {

        if (equippedItems.ContainsKey(_item))
        {
            int _totalAmount = equippedItems[_item] + _amount;
            equippedItems[_item] = _totalAmount;
        }
        else
        {
            equippedItems.Add(_item, _amount);
            UpdateKeysList();
            ActivateNextItem();

        }

        ItemEquipped?.Invoke(_item, equippedItems[_item]);
    }

    private void UpdateKeysList()
    {
        activeItemKeys = equippedItems.Keys.ToList();

    }
    public void UseActiveItem(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {

            if (equippedItems.Count <= 0)
            {
                return;
            }
            Debug.Log("!");

            ConsumeItems(currentActiveItem, 1);
        }
    }
    public void ConsumeItems(Item _item, int _amount)
    {
        int _prevAmount = equippedItems[_item];
        int _finalAmount = _prevAmount - _amount;

        if (_finalAmount == 0)
        {
            equippedItems.Remove(_item);
            UpdateKeysList();

            ActivateNextItem();
        }
        else
        {
            equippedItems[_item] = _finalAmount;
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
        currentActiveIndex = (currentActiveIndex + 1) % activeItemKeys.Count;
        ChangeActiveItem();

        ItemCycled?.Invoke(currentActiveItem, equippedItems[currentActiveItem]);
    }

    public void ActivatePrevItem()
    {
        if (equippedItems.Count <= 0)
        {
            ItemCycled?.Invoke(null, 0);
            return;
        }
        currentActiveIndex = (currentActiveIndex - 1 + activeItemKeys.Count) % activeItemKeys.Count;
        ChangeActiveItem();
        ItemCycled?.Invoke(currentActiveItem, equippedItems[currentActiveItem]);
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
