using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RequirementUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressTXT;
    [SerializeField] private Image reqIcon;
    [SerializeField] private Image reqProgressFill;

    public void SetupRequirement(IRequirement _req, int _cur, int _reqd)
    {
        reqIcon.sprite = _req.GetIcon();
        SetProgress(_cur, _reqd);
    }

    public void SetupObjective(ISublevelObjective _obj, int _cur, int _reqd)
    {
        reqIcon.sprite = _obj.GetIcon();
        SetProgress(_cur, _reqd);
    }

    public void SetProgress(int _cur, int _reqd)
    {
        progressTXT.text = $"{_cur}/{_reqd}";
    }
}