using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HBPointsHUD : MonoBehaviour
{

    public Image hbIMG;
    public TextMeshProUGUI debugTXT;
    public TextMeshProUGUI streakTXT;
    public GameObject streakPanel;
    public HBEnergyIndicators energyIndicators;

    private void Start()
    {
        UpdateFill(PlayerManager.Instance.playerHeadbutt.currentHBpoints, PlayerManager.Instance.playerHeadbutt.maxHBpoints);
    }
    public void UpdateFill(float _current, float _max)
    {
        debugTXT.text = $"{_current}/{_max}";
        energyIndicators.UpdateHBUI(_current, _max);
    }

    public void AnimateBounce()
    {
        Vector3 _bounceScale = new Vector3(1.2f, 1.2f, 2f);
        Tween.StopAll(this.transform);
        Tween.Scale(this.transform, startValue: _bounceScale, endValue: Vector3.one, ease: Ease.OutBack,duration:.3f);
    }

    public void UpdateStreak(int _count)
    {
        if (_count <= 1)
        {
            streakPanel.SetActive(false);
        }
        else
        {
            streakPanel.SetActive(true);
            streakTXT.text = $"x{_count}";
        }
    }

    public void UpdateHBIcon()
    {
        
    }
}
