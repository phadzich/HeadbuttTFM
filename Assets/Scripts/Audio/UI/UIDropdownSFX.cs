using TMPro;
using UnityEngine;

public class UIDropdownSFX : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(int index)
    {
        SoundManager.PlaySound(UIType.BUTTON_CLICK);
    }
}