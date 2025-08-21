using UnityEngine;
using UnityEngine.UI;

public class HBEnergyCounter : MonoBehaviour
{
    public float progress;

    public Color progressColor;
    public Color readyColor;

    public Image progressFill;

    public void UpdateProgress(float _progress)
    {
        progress = _progress;
        if (progress == 1)
        {
            progressFill.color = readyColor;
            progressFill.fillAmount = 1;

        }
        else
        {
            progressFill.fillAmount = progress;
            progressFill.color = progressColor;
        }
    }
}
