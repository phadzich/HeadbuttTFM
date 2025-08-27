using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsHUD : MonoBehaviour
{
    public Image resourceIcon;
    public TextMeshProUGUI amountText;
    public GameObject coinsPanel;
    private int lastAmount;

    public void UpdateAmount(int _amount)
    {
        amountText.text = _amount.ToString();
        if (_amount > lastAmount)
        {
            AnimateAdd();
        }
        else
        {
            AnimateSpend();
        }
        lastAmount = _amount;
    }

    private void AnimateAdd()
    {
        Vector3 _coinScale = new Vector3(2f,2f,2f);
        Vector3 _panelScale = new Vector3(1.2f, 1.2f, 1.2f);
        Tween.Scale(coinsPanel.transform, startValue: _panelScale, endValue: Vector3.one, duration: .8f, ease: Ease.InOutSine);
        Tween.Scale(resourceIcon.transform, startValue: _coinScale, endValue: Vector3.one, duration: 1.2f, ease: Ease.OutBack);
    }
    private void AnimateSpend()
    {
        Vector3 _coinScale = new Vector3(.5f, .5f, .5f);
        Vector3 _panelScale = new Vector3(.8f, .8f, .8f);
        Tween.Scale(coinsPanel.transform, startValue: _panelScale, endValue: Vector3.one, duration: .4f, ease: Ease.InOutSine);
        Tween.Scale(resourceIcon.transform, startValue: _coinScale, endValue: Vector3.one, duration: .8f, ease: Ease.OutBack);
    }
}
