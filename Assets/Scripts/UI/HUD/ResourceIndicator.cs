using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceIndicator : MonoBehaviour
{
    private ResourceData resourceData;
    private int amount;

    public Image resourceIcon;
    public TextMeshProUGUI amountText;

    public void SetupIndicator(ResourceData _resourceData, int _amount)
    {
        this.resourceData = _resourceData;
        this.amount = _amount;
        UpdateUI(_amount);
        if (_amount == 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void UpdateUI(int _amount)
    {
        //resourceIcon.sprite = resourceData.icon;
        resourceIcon.sprite = resourceData.icon;
        amountText.text = _amount.ToString();
    }
}
