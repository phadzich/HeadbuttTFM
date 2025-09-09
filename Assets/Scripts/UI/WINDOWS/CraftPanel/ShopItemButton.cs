using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopItemButton : MonoBehaviour
{
    public Image itemIcon;
    private ShopItem itemData;
    private int itemCount;
    public TextMeshProUGUI amountTXT;
    public TextMeshProUGUI priceTXT;

    public void Setup(ShopItem _shopItem)
    {
        itemData = _shopItem;
        itemCount = itemData.quantity;
        itemIcon.sprite = itemData.item.illustration;
        amountTXT.text = itemCount.ToString();
        priceTXT.text = itemData.price.ToString();
    }

    public void OnClickSelectBtn()
    {
        ShopManager.Instance.currentOpenShop.Sell(itemData, 1);
    }


}
