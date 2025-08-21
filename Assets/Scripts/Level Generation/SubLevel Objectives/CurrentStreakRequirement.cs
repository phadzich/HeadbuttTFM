using UnityEngine;

[System.Serializable]
public class CurrentStreakRequirement : RequirementBase
{
    public int streakNeeded;
    private int actualStreak;

    public override void Initialize(int _id)
    {     
        targetId = _id;
        actualStreak = 0;
        goal = streakNeeded;
        //Debug.Log("INIT");
        //Debug.Log(gateId);
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is MatchStreakEvent streakEvent)
        {
            current = streakEvent.currentStreak;
            Debug.Log($"Streak {current}/{goal} completed {isCompleted}");
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}
