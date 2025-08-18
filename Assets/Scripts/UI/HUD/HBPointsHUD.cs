using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HBPointsHUD : MonoBehaviour
{

    public Image hbIMG;
    public Sprite defaultHBIcon;
    public TextMeshProUGUI hbCost;
    public TextMeshProUGUI debugTXT;
    public TextMeshProUGUI streakTXT;
    public GameObject streakPanel;
    public HBEnergyIndicators energyIndicators;

    public HelmetEffectData currentEffect;
    public int currentEffectTotalCost;

    private void Start()
    {
        UpdateFill(PlayerManager.Instance.playerHeadbutt.currentHBpoints, PlayerManager.Instance.playerHeadbutt.maxHBpoints);
    }
    public void UpdateFill(float _current, float _max)
    {
        debugTXT.text = $"{_current}/{_max}";
        energyIndicators.UpdateHBUI(_current, _max);

        if (currentEffect != null)
        {
            if (PlayerManager.Instance.playerHeadbutt.currentHBpoints < currentEffectTotalCost)
            {
                SetDefaultHBIcon();
            }
            else
            {
                SetSpecialHBIcon();
            }
        }
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

    public void UpdateHBIcon(HelmetInstance _instance)
    {
        if (_instance.activeEffects.Count > 0) //tiene un effect
        {
            currentEffect = _instance.baseHelmet.effects[0];
            currentEffectTotalCost = currentEffect.hbPointsUsed + 1;
            if (_instance.activeEffects[0].hasSpecialAttack && PlayerManager.Instance.playerHeadbutt.currentHBpoints>= currentEffectTotalCost)//tiene un special HB, tiene que ser el primero en la lista
            {
                SetSpecialHBIcon();
            }
            else
            {
                SetDefaultHBIcon();
            }
        }
        else
        {
            currentEffect = null;
            SetDefaultHBIcon();
        }
    }

    private void SetDefaultHBIcon()
    {
        hbIMG.sprite = defaultHBIcon;
        hbCost.text = "1";
    }

    private void SetSpecialHBIcon()
    {
        hbIMG.sprite = currentEffect.effectIcon;
        hbCost.text = (currentEffect.hbPointsUsed + 1).ToString();
    }
}
