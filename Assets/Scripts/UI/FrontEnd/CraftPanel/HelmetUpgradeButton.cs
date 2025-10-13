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
    

    public void ToggleTooltip(bool _enabled)
    {
        text.text = UIManager.Instance.craftingPanel.infoPanel.nextAction;
        tooltip.SetActive(_enabled);
    }
}
