using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HelmetUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    public TextMeshProUGUI text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = UIManager.Instance.craftingPanel.infoPanel.nextAction;
        ToggleTooltip(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleTooltip(false);
    }

    private void ToggleTooltip(bool _enabled)
    {
        tooltip.SetActive(_enabled);
    }
}
