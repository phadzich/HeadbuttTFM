using TMPro;
using UnityEngine;

public class SublevelPanel : MonoBehaviour
{

    public TextMeshProUGUI depthText;
    public TextMeshProUGUI goalsText;
    public Sublevel currentSublevel;
    public int currentDepth;
    public int totalSublevels;
    public void OnGoalsChanged()
    {
        UpdateGoals();
    }
    public void UpdateGoals()
    {
        goalsText.text = $"{currentSublevel.currentBlocksMined}/{currentSublevel.blocksToComplete}";
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
