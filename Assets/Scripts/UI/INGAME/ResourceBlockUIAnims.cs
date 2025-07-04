using PrimeTween;
using System.Collections;
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

    public IEnumerator FlashBlock(Material mat, Color color)
    {
        mat.SetColor("_OverlayColor", color);
        for (float t = 0f; t < 0.3f; t += Time.deltaTime)
        {
            float strength = Mathf.PingPong(t * 10f, 1f); // parpadeo rápido
            mat.SetFloat("_OverlayStrength", strength);
            yield return null;
        }
        mat.SetFloat("_OverlayStrength", 0f); // apaga el overlay
    }
}
