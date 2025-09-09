using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanelUI : MonoBehaviour
{
    Item itemData;

    public TextMeshProUGUI nameTXT;
    public TextMeshProUGUI loreTXT;
    public TextMeshProUGUI effectTXT;
    public TextMeshProUGUI powerTXT;
    public TextMeshProUGUI amountTXT;
    public Image effectIcon;
    public Image itemIcon;

    public void UpdateInfo(Item _itemData)
    {
        this.gameObject.SetActive(true);
        itemData = _itemData;
        UpdateData();
        //UpdateUpgradeButton();

    }

    public void UpdateData()
    {
        nameTXT.text = itemData.itemName;
        loreTXT.text = itemData.itemLore;
        effectTXT.text = itemData.itemDescription.Replace("{{power}}", itemData.value.ToString());
        powerTXT.text = $"+{itemData.value}";
        effectIcon.sprite = itemData.effectIcon;

        itemIcon.sprite = itemData.illustration;
    }


}
