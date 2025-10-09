using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HelmetUpgradeButton : MonoBehaviour
{
    public GameObject tooltip;
    public TextMeshProUGUI text;

    private void OnDisable()
    {
        ToggleTooltip(false);
    }
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = UIManager.Instance.craftingPanel.infoPanel.nextAction;
        ToggleTooltip(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleTooltip(false);
    }
    */

    public void ToggleTooltip(bool _enabled)
    {
        tooltip.SetActive(_enabled);
    }
}
