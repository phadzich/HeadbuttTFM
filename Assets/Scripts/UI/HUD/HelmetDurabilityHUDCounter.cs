using UnityEngine;
using UnityEngine.UI;

public class HelmetDurabilityHUDCounter : MonoBehaviour
{
    public Color availableColor;
    public Color usedColor;
    public Image counterImage;

    public void MakeAvailable()
    {
        counterImage.color = availableColor;
    }

    public void MakeUnavailable()
    {

        counterImage.color = usedColor;
    }
}
