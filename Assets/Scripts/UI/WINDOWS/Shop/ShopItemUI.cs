using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public ShopItem shopItem;

    public TextMeshProUGUI itemNameTXT;
    public Image itemImage;

    public Image resIcon;
   public TextMeshProUGUI resAmountTXT;
    public void SetupItem(ShopItem _shopItem)
    {
        shopItem = _shopItem;
        itemImage.sprite = shopItem.item.illustration;
        itemNameTXT.text = shopItem.item.itemName;
        resIcon.sprite = shopItem.price.resource.icon;
        resAmountTXT.text = shopItem.price.quantity.ToString();
    }

    public void TryBuyItem()
    {
        ShopManager.Instance.currentOpenShop.Sell(shopItem, 1);
    }
}
