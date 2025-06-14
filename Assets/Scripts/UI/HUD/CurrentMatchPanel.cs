using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentMatchPanel : MonoBehaviour
{

    public TextMeshProUGUI resourceNameText;
    public TextMeshProUGUI currentComboText;
    public Image resourceIcon;
    private ResourceData currentResource;

    private float startPosition;

    private void Start()
    {
        startPosition = transform.position.x;
        AnimateBreak();
    }

    public void StartNewCombo(ResourceData _resourceData, int _currentCombo)
    {
        currentResource = _resourceData;
        UpdateComboInfo(_currentCombo);
        AnimateIn();
    }

    public void IncreaseCurrentCombo(int _currentCombo)
    {
        UpdateComboInfo(_currentCombo);
        AnimateIncrease();
    }
    public void EndCurrentCombo()
    {
        AnimateBreak();
    }

    public void CompleteCurrentCombo()
    {
        AnimateCompleted();
    }

    public void ChangeCurrentCombo()
    {
        AnimateBreak();
    }

    public void UpdateComboInfo(int _currentCombo)
    {
        resourceNameText.text = currentResource.shortName;
        currentComboText.text = $"{_currentCombo}/{currentResource.hardness}";
        resourceIcon.sprite = currentResource.icon;
    }

    public void AnimateIn()
    {
        Tween.PositionX(transform, startValue: startPosition + 250, endValue:startPosition,duration:.5f);
    }

    public void AnimateBreak()
    {
        Tween.PositionX(transform, endValue: startPosition + 250, duration: .2f);
        Vector3 _shakeScale = new Vector3(.4f, .4f, 1);
        Tween.ShakeScale(transform, _shakeScale, duration: .2f);
    }

    public void AnimateIncrease()
    {
        Vector3 _growScale = new Vector3(.1f, .1f, 1);
        Tween.PunchScale(transform, _growScale, duration: .15f);
    }

    public void AnimateCompleted()
    {
        Vector3 _growScale = new Vector3(.4f, .4f, 1);
        Tween.PunchScale(transform, _growScale, duration: .5f);
    }
}
