using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public HelmetInventory helmetsInventory;
    public ItemsInventory itemsInventory;


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
