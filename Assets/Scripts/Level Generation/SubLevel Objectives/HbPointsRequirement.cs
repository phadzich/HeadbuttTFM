using UnityEngine;

[System.Serializable]
public class HbPointsRequirement : RequirementBase
{
    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;
    public int HbPointsNeeded;

    public override void Initialize()
    {
        current = 0;
        //current = (int)PlayerManager.Instance.playerHeadbutt.currentHBpoints;
        goal = HbPointsNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is HbPointsEvent hbPointsEvent)
        {
            current = hbPointsEvent.currentPoints;
            //Debug.Log($"HB POINTS {current}/{goal} completed {isCompleted}");
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}
