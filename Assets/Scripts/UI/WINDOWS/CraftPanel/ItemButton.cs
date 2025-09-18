using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemButton : MonoBehaviour
{
    public Image itemIcon;
    private Item itemData;
    private int itemCount;
    public TextMeshProUGUI amountTXT;


    public void Setup(Item _item, int _count)
    {
        itemData = _item;
        itemCount = _count;
        itemIcon.sprite = _item.illustration;
        amountTXT.text = itemCount.ToString();
    }

    public void OnClickSelectBtn()
    {
        UIManager.Instance.InventoryPanel.prompt.SetActive(false);
        UIManager.Instance.InventoryPanel.ItemSelected(itemData);
    }


}
