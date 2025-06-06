using UnityEngine;
using UnityEngine.UI;

public class EquippedHelmetHUDCounter : MonoBehaviour
{
    public Vector3 inactiveSize;
    public Vector3 activeSize;
    public Color activeColor;
    public Color wornColor;
    public Sprite activeIcon;
    public Sprite wornIcon;
    public Image counterImage;
    public HelmetInstance helmetInstance;
    public bool isWornOut;

    public void EquipHelmet(HelmetInstance _inInstance)
    {
        counterImage.sprite = activeIcon;
        counterImage.color = activeColor;
        counterImage.rectTransform.localScale = inactiveSize;
        helmetInstance = _inInstance;
        isWornOut = false;
    }
    public void WearHelmet()
    {
        counterImage.sprite = activeIcon;
        counterImage.color = activeColor;
        counterImage.rectTransform.localScale = activeSize;
    }

    public void UnWearHelmet()
    {
        counterImage.sprite = activeIcon;
        counterImage.color = activeColor;
        counterImage.rectTransform.localScale = inactiveSize;
    }

    public void WornOut()
    {
        isWornOut = true;
        counterImage.sprite = wornIcon;
        counterImage.color = wornColor;
        counterImage.rectTransform.localScale = inactiveSize;
    }
}
