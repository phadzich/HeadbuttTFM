using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shop
{
    public ShopData shopData;
    public int shopID;
    public string shopName;
    public List<ShopItem> shopInventory;

    public Shop(ShopData _shopData)
    {
        shopData = _shopData;
        shopID = shopData.shopID;
        shopName = shopData.shopName;
        shopInventory = shopData.shopItems;
    }
    public void Sell(ShopItem _item, int _quantity)
    {
        int _totalResources = _item.price.quantity * _quantity;

        if (ResourceManager.Instance.SpendResource(_item.price.resource, _totalResources)){
            RemoveFromInventory(_item, _quantity);
        }
        else
        {
            Debug.Log("Not enough resources");
        }
    }
    private void RemoveFromInventory(ShopItem _item, int _quantity)
    {
        /*
        if (shopInventory[_item] < 0) {
            shopInventory.Remove(_item);
        }
        else
        {
            shopInventory[_item] = shopInventory[_item] - _quantity;
        }*/
    }
}