using UnityEngine;
using UnityEngine.UI;

public class UIToggleSFX : MonoBehaviour
{
    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool isOn)
    {
        if (isOn)
            SoundManager.PlaySound(UIType.BUTTON_CLICK);
        else
            SoundManager.PlaySound(UIType.BUTTON_CLICK);
    }
}
