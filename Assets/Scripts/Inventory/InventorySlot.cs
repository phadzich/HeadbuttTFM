using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    [Header("UI")]
    public Button mainButton;
    public TextMeshProUGUI qtyTXT;
    public Image icon;

    [Header("DATA")]
    public Item itemData;
    public int itemCount;
    public HelmetInstance helmetInstance;

    public void Setup(Item _item, int _count)
    {
        itemData = _item;
        itemCount = _count;
        icon.sprite = _item.illustration;
        qtyTXT.text = itemCount.ToString();
    }
}