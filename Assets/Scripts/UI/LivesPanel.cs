using PrimeTween;
using TMPro;
using UnityEngine;

public class LivesPanel : MonoBehaviour
{

    public TextMeshProUGUI livesText;


    public void UpdateLivesInfo(int _currentLives)
    {
        livesText.text = _currentLives.ToString();
        AnimateIncrease();
    }
    private void AnimateIncrease()
    {
        Vector3 _growScale = new Vector3(.1f, .1f, 1);
        Tween.PunchScale(transform, _growScale, duration: .15f);
    }
}
