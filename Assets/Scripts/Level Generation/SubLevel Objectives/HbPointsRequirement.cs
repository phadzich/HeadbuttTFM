using UnityEngine;

[System.Serializable]
public class HbPointsRequirement : RequirementBase
{
    public int HbPointsNeeded;
    private int actualPoints;

    public override void Initialize(int _id)
    {
        targetId = _id;
        actualPoints = 0;
        goal = HbPointsNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is HbPointsEvent hbPointsEvent)
        {
            current = hbPointsEvent.currentPoints;
            Debug.Log($"HB POINTS {current}/{goal} completed {isCompleted}");
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}
