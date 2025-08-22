using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SublevelObjectiveHUDIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressTXT;
    [SerializeField] private Image objIcon;
    [SerializeField] private Image reqProgressFill;

    [SerializeField] Color incompleteColor;
    [SerializeField] Color completeColor;

    public void Setup(Sprite _icon, int _curr, int _goal, float _progress)
    {
        objIcon.sprite = _icon;
        progressTXT.text = $"{_curr}/{_goal}";
        reqProgressFill.fillAmount = _progress;
        if (_curr < _goal)
        {
            reqProgressFill.color = incompleteColor;
            progressTXT.color = incompleteColor;
        }
        else
        {
            reqProgressFill.color = completeColor;
            progressTXT.color = completeColor;
        }
    }
}
