using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;


public class UIPanel : MonoBehaviour
{
    [Header("Slot Tracking")]
    [SerializeField] private GameObject firstSlot;
    [SerializeField] private GameObject lastSelectedSlot;

    private void OnEnable()
    {
        ChangeInputToUI();
        SetSelectedTarget();
    }
    private void OnDisable()
    {
        ChangeInputToPlayer();
        ClearLastSlotVisual();
        UIManager.Instance.DeactivateCurrentCam();
    }

    private void SetSelectedTarget()
    {
        GameObject _target = lastSelectedSlot != null ? lastSelectedSlot : firstSlot;
        if (_target != null)
        {
            EventSystem.current.SetSelectedGameObject(_target);
        }
    }

    public void UpdateLastSelection(GameObject _slot)
    {
        lastSelectedSlot = _slot;
        SetSelectedTarget();
    }

    private void ClearLastSlotVisual()
    {
        if (lastSelectedSlot!=null && lastSelectedSlot.TryGetComponent(out InventorySlot _slot))
        {
            _slot.SetHighlighted(false);
            _slot.SetSelected(false);
        }
    }

    private void ChangeInputToUI()
    {
        InputManager.Instance.playerInput.SwitchCurrentActionMap("UI");
    }

    private void ChangeInputToPlayer()
    {
        InputManager.Instance.playerInput.SwitchCurrentActionMap("Player");
    }


}
