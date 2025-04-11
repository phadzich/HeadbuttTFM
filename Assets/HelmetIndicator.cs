using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetIndicator : MonoBehaviour
{
    public HelmetInstance helmetInstance;

    public Image helmetIcon;
    public TextMeshProUGUI bouncesTxt;
    public Image highlightIndicator;

    
    public void SetupIndicator(HelmetInstance _instance)
    {
        helmetIcon.sprite = _instance.baseHelmet.icon;
        bouncesTxt.text = _instance.remainingBounces.ToString();
        helmetInstance = _instance;
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

}
