using UnityEngine;
public class CollectAnyResourceRequirement : RequirementBase
{
    public int resourcesNeeded;
    private int resourcesCollected;

    public override void Initialize(int _id)
    {
        targetId = _id;
        resourcesCollected = 0;
        goal = resourcesNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is CollectResourceEvent)
            resourcesCollected++;
        current = resourcesCollected;
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}