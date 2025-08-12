using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler


{
    [Header("UI")]
    public Button mainButton;
    public TextMeshProUGUI qtyTXT;
    public Image icon;
    public Image highlightBorder;
    public Image selectionBorder;

    [Header("DATA")]
    public Item itemData;
    public int itemCount;
    public HelmetInstance helmetInstance;
    public SlotType slotType;

    [Header("STATE")]
    public bool isHighlighted;
    public bool isSelected;
    public bool isActive;

    public bool IsOccupied => itemData != null || helmetInstance != null;

    private void Start()
    {
        mainButton.onClick.AddListener(() => InventoryManager.Instance.SelectSlot(this));

    }
    public void AssignItem(Item _item, int _count)
    {
        itemData = _item;
        itemCount = _count;
        icon.sprite = _item.illustration;
        qtyTXT.text = itemCount.ToString();
        slotType = SlotType.Item;
        helmetInstance = null;
    }

    public void EquipItem(Item _item, int _count)
    {
        Debug.Log("SLOT UPDATED!");
        itemData = _item;
        itemCount = _count;
        icon.sprite = _item.illustration;
        qtyTXT.text = itemCount.ToString();
        slotType = SlotType.EquippedItem;
        helmetInstance = null;
    }

    public void AssignHelmet(HelmetInstance _helmet)
    {
        helmetInstance = _helmet;
        slotType = SlotType.Helmet;
        itemData = null;
    }

    public void Clear()
    {
        itemData = null;
        helmetInstance = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        InventoryManager.Instance.HighlightSlot(this);
    }

    public void SetHighlighted(bool value)
    {
        isHighlighted = value;
        highlightBorder.gameObject.SetActive(value);
    }

    public void SetSelected(bool value)
    {
        isSelected = value;
        selectionBorder.gameObject.SetActive(value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isActive) InventoryManager.Instance.SelectSlot(this);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (isActive) InventoryManager.Instance.HighlightSlot(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isActive) InventoryManager.Instance.UnhighlightSlot(this);
    }

    public void SetContextActive(bool _active)
    {
        isActive = _active;
        var _nav = mainButton.navigation;
        _nav.mode = _active ? Navigation.Mode.Automatic : Navigation.Mode.None;
        mainButton.navigation = _nav;
        mainButton.interactable = _active;
        //mainButton.enabled = _active;
    }
}
public enum SlotType { Helmet, Item, EquippedHelmet, EquippedItem }