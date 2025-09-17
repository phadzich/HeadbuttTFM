using UnityEngine;
public class CollectAnyResourceRequirement : RequirementBase
{


    public override Sprite GetIcon() => UIManager.Instance.iconsLibrary.blockReq;
    public int resourcesNeeded;
    private int resourcesCollected;

    public override void Initialize()
    {
        resourcesCollected = 0;
        goal = resourcesNeeded;
        current = 0;
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