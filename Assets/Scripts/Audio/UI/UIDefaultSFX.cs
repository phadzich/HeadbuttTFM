using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDefaultSFX : MonoBehaviour, IPointerEnterHandler
{
    private Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => SoundManager.PlaySound(UIType.BUTTON_CLICK));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.PlaySound(UIType.SELECT2);
    }
}

public enum ButtonType
{
    UI,
    SHOP,
    BUY,
    SWAP,

}