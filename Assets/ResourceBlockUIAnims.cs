using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBlockUIAnims : MonoBehaviour
{

    public GameObject rewardPanelUI;
    public Image resourceIcon;
    public TextMeshProUGUI resourceRewardTXT;

    private void Start()
    {
        rewardPanelUI.SetActive(false);
    }
    public void AnimateResourceRewards(int _amount)
    {
        rewardPanelUI.SetActive(true);
        resourceRewardTXT.text = "+" + _amount.ToString();
        Tween.Scale(rewardPanelUI.transform, endValue:0,duration:.8f,ease:Ease.InBounce);

    }
}
