using UnityEngine;
[System.Serializable]
public class CollectAnyResourceObjective : SublevelObjectiveBase
{

    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;
    public int resourcesNeeded;
    private int resourcesCollected;

    public override void Initialize()
    {
        resourcesCollected = 0;
        goal = resourcesNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is CollectResourceEvent)
            resourcesCollected++;
        current = resourcesCollected;
    }

    public override bool isCompleted => resourcesCollected >= resourcesNeeded;
    public override float progress => (float)resourcesCollected / resourcesNeeded;
}
