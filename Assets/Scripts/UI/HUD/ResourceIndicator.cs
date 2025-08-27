using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceIndicator : MonoBehaviour
{
    private ResourceData resourceData;
    private int amount;

    public GameObject resourceIndicator;
    public Image resourceIcon;
    public TextMeshProUGUI amountText;

    public void SetupIndicator(ResourceData _resourceData, int _amount)
    {
        
        this.resourceData = _resourceData;
        this.amount = _amount;
        amountText.color = resourceData.color;
        resourceIcon.sprite = resourceData.icon;
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
        if(_amount< amount)
        {
            AnimateSpend();
        }
        else if(_amount > amount) 
        {
            AnimateAdd();
        }
        amount = _amount;
        amountText.text = _amount.ToString();
    }

    private void AnimateAdd()
    {
        Vector3 _coinScale = new Vector3(2f, 2f, 2f);
        Vector3 _panelScale = new Vector3(1.2f, 1.2f, 1.2f);
        Tween.Scale(resourceIndicator.transform, startValue: _panelScale, endValue: Vector3.one, duration: .8f, ease: Ease.InOutSine);
        Tween.Scale(resourceIcon.transform, startValue: _coinScale, endValue: Vector3.one, duration: 1.2f, ease: Ease.OutBack);
    }
    private void AnimateSpend()
    {
        Vector3 _coinScale = new Vector3(.5f, .5f, .5f);
        Vector3 _panelScale = new Vector3(.8f, .8f, .8f);
        Tween.Scale(resourceIndicator.transform, startValue: _panelScale, endValue: Vector3.one, duration: .4f, ease: Ease.InOutSine);
        Tween.Scale(resourceIcon.transform, startValue: _coinScale, endValue: Vector3.one, duration: .8f, ease: Ease.OutBack);
    }
}
