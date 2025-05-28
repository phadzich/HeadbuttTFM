using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetIndicator : MonoBehaviour
{
    public HelmetInstance helmetInstance;

    public Image helmetIcon;
    public TextMeshProUGUI durabilityTxt;
    public Image highlightIndicator;

    
    public void SetupIndicator(HelmetInstance _instance)
    {
        helmetIcon.sprite = _instance.baseHelmet.icon;
        durabilityTxt.text = _instance.currentDurability.ToString();
        helmetInstance = _instance;
        highlightIndicator.color = _instance.baseHelmet.color;
    }

    public void UpdateIndicator()
    {
        helmetIcon.sprite = helmetInstance.baseHelmet.icon;
        durabilityTxt.text = helmetInstance.currentDurability.ToString();
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

}
