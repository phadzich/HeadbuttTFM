using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPanelUI : MonoBehaviour
{
    public NavContext activeContext;
    public NavContext prevContext;

    public Transform itemSlotsCont;
    public Transform equippedItemsSlotsCont;
    public Transform helmetSlotsCont;
    public Transform equippedHelmetSlotsCont;
    public void ContextFromSelectedSlot(InventorySlot _slot)
    {
        switch (_slot.slotType)
        {
            case SlotType.Item: SetNavContext(NavContext.EquippedItems); prevContext = NavContext.ItemSlots; break;
            case SlotType.EquippedItem: SetNavContext(NavContext.ItemSlots); prevContext = NavContext.EquippedItems; break;
            case SlotType.Helmet: SetNavContext(NavContext.EquippedHelmets); prevContext = NavContext.HelmetSlots; break;
            case SlotType.EquippedHelmet: SetNavContext(NavContext.HelmetSlots); prevContext = NavContext.EquippedHelmets; break;
        }
    }



    private void SetNavContext(NavContext _newContext)
    {
        activeContext = _newContext;
        switch (_newContext)
        {
            case NavContext.ItemSlots:

                ActivateNavInSlots(itemSlotsCont, true);
                ActivateNavInSlots(equippedItemsSlotsCont, false);
                ActivateNavInSlots(helmetSlotsCont, true);
                ActivateNavInSlots(equippedHelmetSlotsCont, false);
                break;
            case NavContext.EquippedItems:
                ActivateNavInSlots(itemSlotsCont, false);
                ActivateNavInSlots(equippedItemsSlotsCont, true);
                ActivateNavInSlots(helmetSlotsCont, false);
                ActivateNavInSlots(equippedHelmetSlotsCont, false);
                break;
            case NavContext.HelmetSlots:
                ActivateNavInSlots(itemSlotsCont, true);
                ActivateNavInSlots(equippedItemsSlotsCont, false);
                ActivateNavInSlots(helmetSlotsCont, true);
                ActivateNavInSlots(equippedHelmetSlotsCont, false);
                break;
            case NavContext.EquippedHelmets:
                ActivateNavInSlots(itemSlotsCont, false);
                ActivateNavInSlots(equippedItemsSlotsCont, false);
                ActivateNavInSlots(helmetSlotsCont, false);
                ActivateNavInSlots(equippedHelmetSlotsCont, true);
                break;
        }
    }

    private void ActivateNavInSlots(Transform _slotContainer, bool _active)
    {
        foreach (Transform _child in _slotContainer)
        {
            _child.gameObject.GetComponent<InventorySlot>().SetContextActive(_active);
        }

        if (_active)
        {
            EventSystem.current.SetSelectedGameObject(_slotContainer.GetChild(0).gameObject);
        }
    }

}
