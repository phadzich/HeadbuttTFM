using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeButtonUI : MonoBehaviour
{
    public Image resourceIcon;
    public TextMeshProUGUI resourceAmountTXT;
    public ResourceData resourceData;
    public int resourceAmount;

    public void SetupButton(ResourceData _resourceData)
    {
       
        resourceData = _resourceData;

        resourceAmount = ResourceManager.Instance.resourceTrader.ResourcesNeededForUpgradePoint(resourceData);
        Debug.Log(resourceAmount);

        resourceIcon.sprite = resourceData.icon;
        resourceAmountTXT.text = resourceAmount.ToString();
    }
    public void TryExchange()
    {
        ResourceManager.Instance.resourceTrader.BuyUpgradePoint(1, resourceData, resourceAmount);
    }
}
