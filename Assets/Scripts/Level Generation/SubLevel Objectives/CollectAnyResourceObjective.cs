using UnityEngine;
[System.Serializable]
public class CollectAnyResourceObjective : SublevelObjectiveBase
{
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
        Debug.Log($"{current}/{goal}");
    }

    public override bool isCompleted => resourcesCollected >= resourcesNeeded;
    public override float progress => (float)resourcesCollected / resourcesNeeded;
}