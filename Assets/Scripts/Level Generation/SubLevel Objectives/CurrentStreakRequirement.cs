using UnityEngine;

[System.Serializable]
public class CurrentStreakRequirement : RequirementBase
{
    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;
    public int streakNeeded;

    public override void Initialize()
    {     
        current = 0;
        goal = streakNeeded;
        //Debug.Log("INIT");
        //Debug.Log(gateId);
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is MatchStreakEvent streakEvent)
        {
            current = streakEvent.currentStreak;
            //Debug.Log($"Streak {current}/{goal} completed {isCompleted}");
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}
