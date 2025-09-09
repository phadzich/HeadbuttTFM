using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemButton : MonoBehaviour
{
    public Image itemIcon;
    private Item itemData;
    public TextMeshProUGUI amount;

    public void SetUp(Item _itemData)
    {
        itemData = _itemData;
        itemIcon.sprite = itemData.illustration;
        amount.text = InventoryManager.Instance.itemsInventory.ownedItems[_itemData].ToString();
    }
  
    public void OnClickSelectBtn()
    {
        //InventoryManager.Instance.SelectItem(itemData);
    }


}
