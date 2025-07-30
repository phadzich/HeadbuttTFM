using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public Shop shopInstance;
    public Transform shopInventory;
    public ShopItemUI shopItemPrefab;
    public void OpenShop(Shop _shopInstance)
    {
        ShopManager.Instance.currentOpenShop = _shopInstance;
        shopInstance = _shopInstance;
        this.gameObject.SetActive(true);
        Debug.Log("Opening shop" + shopInstance.shopName);
        ClearPreviousItems();
        InstanceInventoryItems();
    }

    private void InstanceInventoryItems()
    {
        foreach (ShopItem _item in shopInstance.shopInventory)
        {
            ShopItemUI _newItem = Instantiate(shopItemPrefab, shopInventory);
            _newItem.SetupItem(_item);
        }
    }

    private void ClearPreviousItems()
    {
        foreach (Transform _child in shopInventory)
        {
            Destroy(_child.gameObject);
        }

    }
}
