using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetIndicator : MonoBehaviour
{
    public HelmetInstance helmetInstance;

    public Image helmetIcon;
    public TextMeshProUGUI durabilityTxt;
    public Image highlightIndicator;
    public TextMeshProUGUI lvlTxt;


    public void SetupIndicator(HelmetInstance _instance)
    {
        helmetIcon.sprite = _instance.baseHelmet.icon;
        durabilityTxt.text = _instance.currentDurability.ToString();
        helmetInstance = _instance;
        lvlTxt.text = "LVL "+helmetInstance.helmetXP.currentSublevel.ToString();
        highlightIndicator.color = _instance.baseHelmet.color;
        highlightIndicator.fillAmount = (float)helmetInstance.helmetXP.currentXP / (float)helmetInstance.helmetXP.XPForNextSublevel();
    }

    public void UpdateIndicator()
    {
        helmetIcon.sprite = helmetInstance.baseHelmet.icon;
        durabilityTxt.text = helmetInstance.currentDurability.ToString();
        lvlTxt.text = "LVL " + helmetInstance.helmetXP.currentSublevel.ToString();
        highlightIndicator.fillAmount = (float)helmetInstance.helmetXP.currentXP / (float)helmetInstance.helmetXP.XPForNextSublevel();
    }

    public void Wear()
    {
        ToggleWearHighlight(1);
    }

    public void Unwear()
    {
        ToggleWearHighlight(.5f);
    }

    private void ToggleWearHighlight(float _alpha)
    {
        //highlightIndicator.gameObject.SetActive(_condition);
        this.GetComponent<CanvasGroup>().alpha = _alpha;
    }

}
