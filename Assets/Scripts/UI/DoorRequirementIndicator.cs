using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorRequirementIndicator : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI text;
    public int requiredBlocks;
    public int currentBlocks;

    public void SetupIndicator(int _required, int _current)
    {
        requiredBlocks = _required;
        currentBlocks = _current;
        text.text = $"{_current.ToString()}/{_required.ToString()}";
    }
    public void UpdateIndicator(int _current)
    {
        text.text = $"{_current.ToString()}/{requiredBlocks.ToString()}";

    }
}
