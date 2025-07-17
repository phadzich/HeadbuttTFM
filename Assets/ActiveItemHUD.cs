using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemHUD : MonoBehaviour
{
    public Image itemIconIMG;
    public TextMeshProUGUI itemQuantityTXT;

public void ChangeActiveItem(Item _itemData, int _quantity)
    {
        this.gameObject.SetActive(true);
        itemIconIMG.sprite = _itemData.illustration;
        itemQuantityTXT.text = _quantity.ToString();
    }

    public void DisableUI()
    {
        this.gameObject.SetActive(false);
    }
}
