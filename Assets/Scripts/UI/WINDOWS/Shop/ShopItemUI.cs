using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public ShopItem shopItem;

    public TextMeshProUGUI itemNameTXT;
    public Image itemImage;

    public TextMeshProUGUI coinsPrice;
    public void SetupItem(ShopItem _shopItem)
    {
        shopItem = _shopItem;
        itemImage.sprite = shopItem.item.illustration;
        itemNameTXT.text = shopItem.item.itemName;
        coinsPrice.text = shopItem.price.ToString();
    }

    public void TryBuyItem()
    {
        ShopManager.Instance.currentOpenShop.Sell(shopItem, 1);
    }
}
