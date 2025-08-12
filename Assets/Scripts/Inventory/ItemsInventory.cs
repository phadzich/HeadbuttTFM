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
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] public Transform slotContainer;

    [SerializeField] private GameObject firstSlot;

    private int maxItemsPerSlot = 3;

    public Item currentActiveItem;
    public int currentActiveIndex;
    public List<Item> equippedItemsList;

    public UIPanel inventoryPanel;

    [SerializeField]
    public Dictionary<Item, int> ownedItems;
    [SerializeField]
    public Dictionary<Item, int> equippedItems;

    public Action<Item, int> ItemEquipped;
    public Action<Item, int> ItemConsumed;
    public Action<Item, int> ItemCycled;

    public void Init()
    {
        ownedItems = new Dictionary<Item, int>();
        equippedItems = new Dictionary<Item, int>();
    }

    public void OpenUI()
    {
        RefreshInventoryUI();
        inventoryPanel.UpdateLastSelection(firstSlot);
    }

    public void CloseUI()
    {
        inventoryPanel.UpdateLastSelection(firstSlot);
    }
    public void ChangeActiveItem()
    {
        if (equippedItemsList.Count > 0)
        {
            currentActiveItem = equippedItemsList[currentActiveIndex];
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
 
            //ActivateNextItem();
        }
        RefreshInventoryUI();

    }
    public void TryEquipItem(Item _item, int _amount,InventorySlot _slot)
    {
        Debug.Log(_item.itemName);
        
        if (!equippedItems.ContainsKey(_item))
        {
            Debug.Log("ITEM NOT EQUIPPED");
            int _totalAmount = ownedItems[_item];
            int _finalQtyEquipped = _amount;
            //SE EXCEDE, REGRESAMOS EL EXCEDENTE y solo depositamos el max
            if (_totalAmount > maxItemsPerSlot)
            {
                _finalQtyEquipped = maxItemsPerSlot;
                var _returnAmount = _totalAmount - maxItemsPerSlot;
                UpdateOwnedItem(_item,_returnAmount);
            }
            //ES EXACTO o MENOR, ELIMINAMOS EL ITEM DEL EQUIPPED
            else
            {
                RemoveOwnedItem(_item);
            }
            EquipNewItem(_item, _finalQtyEquipped);
            InventoryManager.Instance.UpdateEquippedSlotData(_item, _finalQtyEquipped, _slot);
        }
    
        
        /*
        if (equippedItems.ContainsKey(_item))
        {
            Debug.Log("ITEM ALREADY EQUIPPED");
            //EL TOTAL
            int _totalAmount = equippedItems[_item] + _amount;
            //SE EXCEDE, REGRESAMOS EL EXCEDENTE
            if(_totalAmount> maxItemsPerSlot)
            {
                var _returnAmount = _totalAmount - maxItemsPerSlot;
                equippedItems[_item] = maxItemsPerSlot;
                ownedItems[_item] = _returnAmount;
            }
            //ES EXACTO, ELIMINAMOS EL ITEM DEL EQUIPPED
            else if(_totalAmount == maxItemsPerSlot)
            {
                ownedItems.Remove(_item);
            }
            //ES MENOR AL MAX, SOLO CAMBIAMOS EL AMOUNT
            else
            {
                equippedItems[_item] = _totalAmount;
            }
            InventoryManager.Instance.UpdateEquippedSlotData(_item, _totalAmount, _slot);
        }

        */
        ItemEquipped?.Invoke(_item, equippedItems[_item]);
        RefreshInventoryUI();
        UpdateKeysList();
        ActivateNextItem();
    }


    private void EquipNewItem(Item _newItem, int _amount)
    {
        equippedItems.Add(_newItem, maxItemsPerSlot);
    }

    private void UpdateOwnedItem(Item _ownedItem, int _amount)
    {
        ownedItems[_ownedItem] = _amount;
    }

    private void RemoveOwnedItem(Item _ownedItem)
    {
        ownedItems.Remove(_ownedItem);
    }

    private void UpdateKeysList()
    {
        equippedItemsList = equippedItems.Keys.ToList();

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
        currentActiveIndex = (currentActiveIndex + 1) % equippedItemsList.Count;
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
        currentActiveIndex = (currentActiveIndex - 1 + equippedItemsList.Count) % equippedItemsList.Count;
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

            if (firstChild.TryGetComponent(out InventorySlot slot))
            {
                firstSlot = firstChild.gameObject;
                inventoryPanel.UpdateLastSelection(firstSlot);
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
