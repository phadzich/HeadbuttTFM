using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetHUD : MonoBehaviour
{
    public HelmetInstance helmetInstance;
    public Image helmetIMG;
    public Image selectionBG;
    public Image selectedArrowIMG;
    public Image faderIMG;
    public Image miningPowerIMG;
    public TextMeshProUGUI durabilityTXT;
    public Image durabilityFillIMG;
    public GameObject evolvePanel;
    public List<Sprite> miningPowerSprites;

    public Image res01IMG;
    public TextMeshProUGUI res01TXT;
    public Image res02IMG;
    public TextMeshProUGUI res02TXT;

    public Image upgradeReadyIndicatorIMG;
    public Image upgradeReadyVisibleIndicatorIMG;

    public Color selectedColor;
    public Color brokenColor;
    public Color unselectedColor;
    public Color notUpgreadableColor;
    public Color upgreadableColor;
    public void LoadHelmet(HelmetInstance _helmetInstance)
    {
        helmetInstance = _helmetInstance;
        helmetIMG.sprite = helmetInstance.baseHelmet.icon;
        miningPowerIMG.sprite = miningPowerSprites[(int)_helmetInstance.baseHelmet.miningPower];
        UpdateDurability(_helmetInstance.currentDurability, _helmetInstance.durability);
        UnWearHelmet();
    }

    public void UpdateDurability(int _current, int _max)
    {
        float _fillAmount = ((float)_current / (float)_max);
        durabilityTXT.text = $"{_current}/{_max}";
        durabilityFillIMG.fillAmount = _fillAmount;
    }

    public void UpdateResourcesNeeded()
    {
        UpgradeRequirement _requirement = helmetInstance.GetUpgradeRequirement(helmetInstance.nextLevel);

        int _res01Q = _requirement.requirements[0].quantity;
        int _res02Q = _requirement.requirements[1].quantity;

        ResourceData _res01D = _requirement.requirements[0].resource;
        ResourceData _res02D = _requirement.requirements[1].resource;

        int _res01Curr = ResourceManager.Instance.ownedResources[_res01D];
        int _res02Curr = ResourceManager.Instance.ownedResources[_res02D];


        res01IMG.sprite = _requirement.requirements[0].resource.icon;
        res01TXT.text = $"{_res01Curr}/{_res01Q}";

        res02IMG.sprite = _requirement.requirements[1].resource.icon;
        res02TXT.text = $"{_res02Curr}/{_res02Q}";

        if (_res01Q <= _res01Curr && _res02Q <= _res02Curr)
        {
            upgradeReadyVisibleIndicatorIMG.color = upgreadableColor;
            upgradeReadyVisibleIndicatorIMG.gameObject.SetActive(true);
            upgradeReadyIndicatorIMG.color = upgreadableColor;
        }
        else
        {
            upgradeReadyVisibleIndicatorIMG.color = upgreadableColor;
            upgradeReadyVisibleIndicatorIMG.gameObject.SetActive(false);
            upgradeReadyIndicatorIMG.color = notUpgreadableColor;
        }
    }
    public void WearHelmet()
    {
        selectionBG.color = selectedColor;
        selectedArrowIMG.gameObject.SetActive(true);
    }

    public void Broken()
    {

        selectionBG.color = brokenColor;
        faderIMG.gameObject.SetActive(true);
        selectedArrowIMG.gameObject.SetActive(false);
    }

    public void UnWearHelmet()
    {
        selectionBG.color = unselectedColor;
        selectedArrowIMG.gameObject.SetActive(false);
    }

}
