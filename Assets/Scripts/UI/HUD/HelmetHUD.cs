using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelmetHUD : MonoBehaviour
{
    public HelmetInstance helmetInstance;
    public Image selectionBG;
    public Image selectedArrowIMG;
    public Image faderIMG;
    public Image miningPowerIMG;

    public List<Sprite> miningPowerSprites;

    public Color selectedColor;
    public Color brokenColor;
    public Color unselectedColor;
    public void LoadHelmet(HelmetInstance _helmetInstance)
    {
        helmetInstance = _helmetInstance;
        miningPowerIMG.sprite = miningPowerSprites[(int)_helmetInstance.baseHelmet.miningPower];
        UnWearHelmet();
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
