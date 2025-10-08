using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDefaultSFX : MonoBehaviour, IPointerEnterHandler
{
    private Button _button;

    public ButtonType type;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        SetUp();
    }

    private void SetUp()
    {
        switch (type)
        {
            case ButtonType.UI:
                _button.onClick.AddListener(() => PlayClick(UIType.BUTTON_CLICK));
                break;
            case ButtonType.SHOP:
                _button.onClick.AddListener(() => PlayClick(UIType.BUY));
                break;
            case ButtonType.HELMET:
                _button.onClick.AddListener(() => PlayClick(UIType.SELECT_HELMET));
                break;
            case ButtonType.SWAP:
                _button.onClick.AddListener(() => PlayClick(UIType.SWAP));
                break;
        }
    }

    private void PlayClick(UIType _type)
    {
        SoundManager.PlaySound(_type, 0.7f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;

        SoundManager.PlaySound(UIType.SELECT2);
    }
}

public enum ButtonType
{
    UI,
    SHOP,
    HELMET,
    SWAP,

}