using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPPanel : MonoBehaviour
{
    public Image XPFillBar;
    public TextMeshProUGUI currentLVLtxt;
    public TextMeshProUGUI currentXPtxt;

    private void Start()
    {
        XPFillBar.fillAmount = 0;
        currentXPtxt.text = $"0/{XPManager.Instance.baseRequiredXP}";
        currentLVLtxt.text = $"LVL 1";
    }
    public void UpdateXP(int _current,  int _max)
    {
        XPFillBar.fillAmount = (float)_current / _max;
        currentXPtxt.text = $"{_current}/{_max}";
    }

    public void UpdateLVL(int _currentLVL)
    {
        currentLVLtxt.text = $"LVL {_currentLVL}";
    }


}
