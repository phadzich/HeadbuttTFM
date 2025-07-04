using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GateRequirementIndicator : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI text;
    public int requiredBlocks;
    public int currentBlocks;

    public void SetupIndicator(int _required, int _current, Sprite _resSprite)
    {
        requiredBlocks = _required;
        currentBlocks = _current;
        icon.sprite = _resSprite;
        text.text = $"{_current.ToString()}/{_required.ToString()}";
    }
    public void UpdateIndicator(int _current)
    {
        text.text = $"{_current.ToString()}/{requiredBlocks.ToString()}";

    }
}
