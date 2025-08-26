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

        resourceAmount = ResourceManager.Instance.coinTrader.ResourcesNeededForCoin(resourceData);
        //Debug.Log(resourceAmount);

        resourceIcon.sprite = resourceData.icon;
        resourceAmountTXT.text = resourceAmount.ToString();
        UpdateButtonStatus();

    }
    public void TryExchange()
    {
        ResourceManager.Instance.coinTrader.BuyCoin(1, resourceData, resourceAmount);
        UpdateButtonStatus();
    }

    private void EnableButton(bool _value)
    {
        if (_value == true)
        {
            this.GetComponent<Button>().interactable = true;

        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

    private void UpdateButtonStatus()
    {
        if (ResourceManager.Instance.ownedResources.ContainsKey(resourceData))
        {
            //Debug.Log($"Req:{resourceAmount}  Res: {ResourceManager.Instance.ownedResources[resourceData]}");
            if (resourceAmount <= ResourceManager.Instance.ownedResources[resourceData])
            {
                EnableButton(true);
            }
            else
            {
                EnableButton(false);
            }

        }
        else
        {
            EnableButton(false);
        }
    }
}
