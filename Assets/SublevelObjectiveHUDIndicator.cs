using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SublevelObjectiveHUDIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressTXT;
    [SerializeField] private Image objIcon;
    [SerializeField] private Image reqProgressFill;

    public void Setup(Sprite _icon, int _curr, int _goal, float _progress)
    {
        progressTXT.text = $"{_curr}/{_goal}";
        objIcon.sprite = _icon;
    }
}
