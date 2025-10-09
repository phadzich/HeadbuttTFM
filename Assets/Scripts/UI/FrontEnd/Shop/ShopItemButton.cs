using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopItemButton : MonoBehaviour
{
    public Image itemIcon;
    public ShopItem itemData;
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
        UpdateButtonStatus();
    }

    public void OnClickSelectBtn()
    {
        ShopManager.Instance.currentOpenShop.Sell(itemData, 1);
        UpdateButtonStatus();
    }

    private void UpdateButtonStatus()
    {
        if (ResourceManager.Instance.coinTrader.HasEnoughCoins(itemData.price))
        {
            EnableButton(true);
        }
        else
        {
            EnableButton(false);
        }
    }
    private void EnableButton(bool _value)
    {
            this.GetComponent<Button>().interactable = _value;
    }
}
