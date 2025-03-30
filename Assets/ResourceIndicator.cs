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
    }

    public void UpdateUI(int _amount)
    {
        //resourceIcon.sprite = resourceData.icon;
        resourceIcon.color = new Color(resourceData.color.r, resourceData.color.g, resourceData.color.b, 1);
        amountText.text = _amount.ToString();
    }
}
