using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorRequirementIndicator : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI text;
    public int requiredInt;
    public int currentInt;


    public Sprite mineIcon;
    public Sprite keysIcon;
    public Sprite exploreIcon;
    public Sprite checkpointIcon;

    public void SetupIndicator(int _current, int _required, SublevelGoalType _goalType)
    {
        requiredInt = _required;
        currentInt = _current;

        switch (_goalType)
        {
            case SublevelGoalType.MineBlocks:
                icon.sprite = mineIcon;
                break;
            case SublevelGoalType.CollectKeys:
                icon.sprite = keysIcon;
                break;
            case SublevelGoalType.Open:
                icon.sprite = exploreIcon;
                break;
        }

        text.text = $"{_current.ToString()}/{_required.ToString()}";
    }
    public void UpdateIndicator(int _current)
    {
        text.text = $"{_current.ToString()}/{requiredInt.ToString()}";

    }
}
