using UnityEngine;
using UnityEngine.UI;

public class SpecialHeadbuttHUD : MonoBehaviour
{

    public Image specialIconIMG;
    public Image blackFader;

    private void Start()
    {
        HideIcon();
    }
    public void ShowIcon()
    {
        specialIconIMG.gameObject.SetActive(true);
    }

    public void HideIcon()
    {
        specialIconIMG.gameObject.SetActive(false);
    }

    public void FadeIcon(float _alpha)
    {
        blackFader.color = new Color(blackFader.color.r, blackFader.color.g, blackFader.color.b, _alpha);
    }

}
