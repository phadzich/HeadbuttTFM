using UnityEngine;
public class CollectAnyResourceRequirement : RequirementBase
{
    

    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;
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