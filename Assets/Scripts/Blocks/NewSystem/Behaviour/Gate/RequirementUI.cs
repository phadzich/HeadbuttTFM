using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RequirementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressTXT;
    [SerializeField] private Image reqIcon;
    [SerializeField] private Image reqProgressFill;
    [SerializeField] Color incompleteColor;
    [SerializeField] Color completeColor;

    public void SetupRequirement(IRequirement _req, int _cur, int _reqd)
    {
        var _progress = _req.progress;
        reqIcon.sprite = _req.GetIcon();
        SetProgress(_cur, _reqd, _progress);
    }

    public void SetupObjective(ISublevelObjective _obj, int _cur, int _reqd)
    {
        var _progress = _obj.progress;
        reqIcon.sprite = _obj.GetIcon();
        SetProgress(_cur, _reqd, _progress);
    }

    public void SetProgress(int _cur, int _reqd, float _progress)
    {
        progressTXT.text = $"{_cur}/{_reqd}";
        reqProgressFill.fillAmount = _progress;
        if (_cur < _reqd)
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