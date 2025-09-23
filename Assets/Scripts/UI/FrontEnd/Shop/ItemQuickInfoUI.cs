using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuickInfoUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI itemOwned;

    public void ShowInfo(ShopItem _shopItem)
    {
        ShowPanel();
        itemName.text = _shopItem.item.itemName;
        string _desc = _shopItem.item.itemDescription;
        _desc = _desc.Replace("{{power}}", _shopItem.item.value.ToString());
        itemDescription.text = _desc;
        int _owned = 0;
        if (InventoryManager.Instance.itemsInventory.ownedItems.ContainsKey(_shopItem.item))
        {
            _owned = InventoryManager.Instance.itemsInventory.ownedItems[_shopItem.item];
        }

        itemIcon.sprite = _shopItem.item.illustration;
        itemOwned.text = $"OWNED: {_owned}";
    }

    public void ShowPanel()
    {
        this.gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }
}
