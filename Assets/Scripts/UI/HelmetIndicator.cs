using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetIndicator : MonoBehaviour
{
    public HelmetInstance helmetInstance;

    public Image helmetIcon;
    public TextMeshProUGUI bouncesTxt;
    public Image highlightIndicator;
    public GameObject upgradeButton;

    public TextMeshProUGUI req1Txt;
    public Image req1Icon;

    public TextMeshProUGUI req2Txt;
    public Image req2Icon;

    public void SetupIndicator(HelmetInstance _instance)
    {
        helmetIcon.sprite = _instance.baseHelmet.icon;
        bouncesTxt.text = _instance.remainingBounces.ToString();
        helmetInstance = _instance;
        highlightIndicator.color = _instance.baseHelmet.color;
    }

    public void UpdateIndicator()
    {
        helmetIcon.sprite = helmetInstance.baseHelmet.icon;
        bouncesTxt.text = helmetInstance.remainingBounces.ToString();
    }

    public void Wear()
    {
        ToggleWearHighlight(true);
    }

    public void Unwear()
    {
        ToggleWearHighlight(false);
    }

    private void ToggleWearHighlight(bool _condition)
    {
        highlightIndicator.gameObject.SetActive(_condition);
    }

    public void ShowUpgradeButton(bool _condition)
    {
        upgradeButton.SetActive(_condition);
    }

    public void UpgradeHelmet()
    {
        CraftingManager.Instance.UpgradeHelmet(helmetInstance);
    }

   public void UpdateReq01(ResourceRequirement _req)
    {
        req1Icon.sprite = _req.resource.icon;
        req1Txt.text = (_req.quantity* (helmetInstance.level + 1)).ToString();
    }

    public void UpdateReq02(ResourceRequirement _req)
    {
        if (_req!=null)
        {
            req2Icon.sprite = _req.resource.icon;
            req2Txt.text = (_req.quantity * (helmetInstance.level+1)).ToString();
        }
        else
        {
            req2Icon.sprite = null;
            req2Txt.text = "";
        }

    }

}
