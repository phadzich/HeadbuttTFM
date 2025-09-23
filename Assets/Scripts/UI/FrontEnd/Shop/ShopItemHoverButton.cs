using UnityEngine;
using UnityEngine.UI;

public class ShopItemHoverButton : Button
{
    public System.Action onHoverEnter;
    public System.Action onHoverExit;

    private bool isHovered = false;
    private bool isPressed = false;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        if (state == SelectionState.Highlighted && !isHovered)
        {
            isHovered = true;
            UIManager.Instance.shopPanel.quickInfoPanel.ShowInfo(GetComponent<ShopItemButton>().itemData);
        }
        else if (state != SelectionState.Highlighted && isHovered)
        {
            isHovered = false;
            UIManager.Instance.shopPanel.quickInfoPanel.HidePanel();
        }
        if (state == SelectionState.Pressed && !isPressed)
        {
            isPressed = true;
            UIManager.Instance.shopPanel.quickInfoPanel.ShowInfo(GetComponent<ShopItemButton>().itemData);
        }
        else if (state != SelectionState.Pressed && isPressed)
        {
            isPressed = false;
        }
    }
}