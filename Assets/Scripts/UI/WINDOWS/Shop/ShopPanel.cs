using System;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public Shop shopInstance;
    public ShopItemsPanelUI shopItems;
    public ShopItemUI shopItemPrefab;
    public CoinsPanelUI coinsPanel;

    private void OnEnable()
    {
        ResourceManager.Instance.coinTrader.onCoinsChanged += OnCoinsChanged;
        ShopManager.Instance.currentOpenShop.shopItemsChanged += OnItemsChanged;
}
    private void OnDisable()
    {
        ResourceManager.Instance.coinTrader.onCoinsChanged -= OnCoinsChanged;
        ShopManager.Instance.currentOpenShop.shopItemsChanged -= OnItemsChanged;
    }

    public void OnCoinsChanged(int _coins)
    {
        shopItems.UpdateInfo(shopInstance.shopInventory);
        coinsPanel.UpdateInfo();
    }

    public void OnItemsChanged()
    {
        shopItems.UpdateInfo(shopInstance.shopInventory);
    }
    public void OpenShop(Shop _shopInstance)
    {
        ShopManager.Instance.currentOpenShop = _shopInstance;
        shopInstance = _shopInstance;
        this.gameObject.SetActive(true);
        Debug.Log("Opening shop" + shopInstance.shopName);

        shopItems.UpdateInfo(shopInstance.shopInventory);
    }

}
