using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SublevelPanel : MonoBehaviour
{
    SublevelGoalType currentGoalType;
    public Image goalIcon;
    public TextMeshProUGUI depthText;
    public TextMeshProUGUI goalsText;
    public Sublevel currentSublevel;
    public int currentDepth;
    public int totalSublevels;

    public Sprite mineIcon;
    public Sprite keysIcon;
    public Sprite exploreIcon;
    public Sprite checkpointIcon;

    public Color incompleteColor;
    public Color completeColor;

    public void ChangeGoalType(SublevelGoalType _goalType)
    {
        currentGoalType = _goalType;
        switch (_goalType)
        {
            case SublevelGoalType.MineBlocks:
                goalIcon.sprite = mineIcon;
                break;
            case SublevelGoalType.CollectKeys:
                goalIcon.sprite = keysIcon;
                break;
            case SublevelGoalType.Open:
                goalIcon.sprite = exploreIcon;
                break;
        }
    }
    public void OnGoalsChanged()
    {
        UpdateGoals();
    }
    public void UpdateGoals()
    {
        
    }

    public void CheckIfGoalCompleted(int _a, int _b)
    {
        if (_a >= _b)
        {
            goalsText.color = completeColor;
            goalIcon.color = completeColor;
        }
        else
        {
            goalsText.color = incompleteColor;
            goalIcon.color = incompleteColor;
        }
    }

    public void ShowCheckpoint()
    {
        goalIcon.sprite = checkpointIcon;
        goalsText.text = "CHECKPOINT";
    }
    public void UpdateDepth()
    {
        totalSublevels = LevelManager.Instance.currentLevel.config.subLevels.Count;
        currentDepth = LevelManager.Instance.currentLevelDepth+1;
        depthText.text = $"{currentDepth}/{totalSublevels}";
    }

    public void UpdateSublevel()
    {
        currentSublevel = LevelManager.Instance.currentSublevel;
        UpdateDepth();
        UpdateGoals();
    }

}
