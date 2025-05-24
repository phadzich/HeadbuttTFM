using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemainingBlocksIndicator : MonoBehaviour
{
    public Image resourceIcon;
    public TextMeshProUGUI remainingText;

    private void Start()
    {
        ToggleIndicator(false);
    }
    public void UpdateIndicator(ResourceData _resource, int _remaining)
    {
        resourceIcon.sprite = _resource.icon;
        remainingText.text = _remaining.ToString();
    }

    public void UpdateIndicatorCount(int _remaining)
    {
        remainingText.text = _remaining.ToString();
    }


    public void ToggleIndicator(bool _visible)
    {
        this.gameObject.SetActive(_visible);
    }
}
