using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
        shopInventory = shopInventory = shopData.shopItems.Select(item => item.Clone()).ToList();
    }


    public void Sell(ShopItem _item, int _quantity)
    {
        int _totalResources = _item.price.quantity * _quantity;

        if (ResourceManager.Instance.SpendResource(_item.price.resource, _totalResources)){
            RemoveFromInventory(shopInventory.IndexOf(_item), _quantity);
            InventoryManager.Instance.itemsInventory.TryEquipItems(_item.item, _quantity);
        }
        else
        {
            Debug.Log("Not enough resources");
        }
    }
    private void RemoveFromInventory(int _itemIndex, int _quantity)
    {
        //Debug.Log(_itemIndex);
        if (_itemIndex < 0)
        {
            return;
        }
        ShopItem _itemToRemove = shopInventory[_itemIndex];
        int _availableQuantity = shopInventory[_itemIndex].quantity;
        if (_availableQuantity <= _quantity)
        {
            shopInventory.Remove(_itemToRemove);
        }
        else
        {
            _itemToRemove.quantity-= _quantity;
        }
    }
}