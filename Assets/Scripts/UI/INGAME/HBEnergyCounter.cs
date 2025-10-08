using UnityEngine;
using UnityEngine.UI;

public class HBEnergyCounter : MonoBehaviour
{
    public float progress;

    public Color progressColor;
    public Color readyColor;
    public Animator VFXanimator;
    public bool hasChanged;
    public Image progressFill;

    public void UpdateProgress(float _progress)
    {
        if (progress != _progress)
        {
            hasChanged = true;
        }
        else
        {
            hasChanged = false;
        }

            progress = _progress;
        if (progress == 1)
        {
            progressFill.color = readyColor;
            progressFill.fillAmount = 1;
            if (hasChanged)
            {
                VFXanimator.Play("Helmet_Healed");
            }
        }
        else
        {
            progressFill.fillAmount = progress;
            progressFill.color = progressColor;
        }
    }
}
