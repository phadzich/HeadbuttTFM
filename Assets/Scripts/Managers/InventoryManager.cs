using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public HelmetInventory helmetsInventory;
    public ItemsInventory itemsInventory;

    [SerializeField]private InventorySlot highlightedSlot;
    [SerializeField] private InventorySlot selectedSlot;

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

        selectedSlot = _slot;
        selectedSlot.SetSelected(true);
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
        helmetsInventory.Init();
        itemsInventory.Init();
    }


}
