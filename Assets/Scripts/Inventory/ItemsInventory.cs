using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemsInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] public Transform slotContainer;

    [SerializeField] private GameObject firstSlot;

    public Item currentActiveItem;
    public int currentActiveIndex;
    public List<Item> equippedItems;

    public Dictionary<Item, int> ownedItems;

    public Action<Item, int> ItemEquipped;
    public Action<Item, int> ItemConsumed;
    public Action<Item, int> ItemCycled;

    public void Init()
    {
        ownedItems = new Dictionary<Item, int>();
    }

    public void OpenUI()
    {
        EventSystem.current.SetSelectedGameObject(firstSlot);
        Debug.Log(EventSystem.current.currentSelectedGameObject?.name);
    }

    public void CloseUI()
    {
        firstSlot = EventSystem.current.currentSelectedGameObject;
    }
    public void ChangeActiveItem()
    {
        currentActiveItem = equippedItems[currentActiveIndex];

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
            UpdateKeysList();
            ActivateNextItem();

        }
        RefreshInventoryUI();
        ItemEquipped?.Invoke(_item, ownedItems[_item]);
    }

    private void UpdateKeysList()
    {
        equippedItems = ownedItems.Keys.ToList();

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
            UpdateKeysList();

            ActivateNextItem();
        }
        else
        {
            ownedItems[_item] = _finalAmount;
            ItemConsumed?.Invoke(_item, _finalAmount);
        }

        _item.Use();
        RefreshInventoryUI();
    }

    public void NextEquippedItem(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {

            if (ownedItems.Count <= 1) return;
            ActivateNextItem();
        }
    }

    public void ActivateNextItem()
    {

        if (ownedItems.Count <= 0)
        {
            ItemCycled?.Invoke(null, 0);
            return;
        }
        currentActiveIndex = (currentActiveIndex + 1) % equippedItems.Count;
        ChangeActiveItem();

        ItemCycled?.Invoke(currentActiveItem, ownedItems[currentActiveItem]);
    }

    public void ActivatePrevItem()
    {
        if (ownedItems.Count <= 0)
        {
            ItemCycled?.Invoke(null, 0);
            return;
        }
        currentActiveIndex = (currentActiveIndex - 1 + equippedItems.Count) % equippedItems.Count;
        ChangeActiveItem();
        ItemCycled?.Invoke(currentActiveItem, ownedItems[currentActiveItem]);
    }

    public void PreviousEquippedItem(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {

            if (ownedItems.Count <= 1) return;
            ActivatePrevItem();
        }
    }


    public void RefreshInventoryUI()
    {

        foreach (Transform _child in slotContainer)
        {
            Destroy(_child.gameObject);
        }

        foreach (var _kvp in ownedItems)
        {
            var item = _kvp.Key;
            var count = _kvp.Value;

            var _slotGO = Instantiate(inventorySlotPrefab, slotContainer);
            var _slot = _slotGO.GetComponent<InventorySlot>();
            _slot.AssignItem(item, count);
        }
        StartCoroutine(AssignFirstSlotNextFrame());

    }
    private IEnumerator AssignFirstSlotNextFrame()
    {
        yield return null; // espera un frame para que Unity procese destrucción y layout

        if (slotContainer.childCount > 0)
        {
            Transform firstChild = slotContainer.GetChild(0);
            Debug.Log("First child after refresh: " + firstChild.name);

            if (firstChild.TryGetComponent(out InventorySlot slot))
            {
                firstSlot = firstChild.gameObject;
                Debug.Log("FIRSTSLOT");
            }
            else
            {
                Debug.Log("NOSLOT");
            }
        }
        else
        {
            Debug.LogWarning("No children found in slotContainer after refresh.");
        }

    }
    }
