using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public List<ShopData> shopsData;
    [SerializeField]
    public List<Shop> shopInstances;
    public Shop currentOpenShop;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("ShopManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateShopsFromData();
    }
    private void CreateShopsFromData()
    {
        foreach (ShopData _data in shopsData)
        {
            CreateShopInstance(_data);
        }
    }
    public void CreateShopInstance(ShopData _data)
    {
        Shop newShop = new Shop(_data);
        shopInstances.Add(newShop);
    }
    
    public Shop ShopById(int _id)
    {
        foreach(Shop _shop in shopInstances)
        {
            if(_id == _shop.shopID) return _shop;
        }
        return null;
    }

}
