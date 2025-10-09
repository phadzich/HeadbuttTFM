using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItemHUD : MonoBehaviour
{
    public Image itemIconIMG;
    public TextMeshProUGUI itemQuantityTXT;
    public Animator VFX;

    private void Start()
    {
        DisableUI();
    }
    public void ChangeActiveItem(Item _itemData, int _quantity)
    {
        this.gameObject.SetActive(true);
        itemIconIMG.sprite = _itemData.illustration;
        itemQuantityTXT.text = _quantity.ToString();
    }

    public void VFXConsume()
    {
        VFX.Play("Item_Consumed");
    }

    public void DisableUI()
    {
        this.gameObject.SetActive(false);
    }
}
