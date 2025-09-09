using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquippedItemButton : MonoBehaviour
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

    public void OnClickSwapBtn()
    {
        InventoryManager.Instance.itemsInventory.SwapHelmet(UIManager.Instance.InventoryPanel.currentSelectedItem, itemData);
        UIManager.Instance.InventoryPanel.OnItemsListChanged();
        UIManager.Instance.InventoryPanel.ToggleSwapPanel(false);
    }
}
