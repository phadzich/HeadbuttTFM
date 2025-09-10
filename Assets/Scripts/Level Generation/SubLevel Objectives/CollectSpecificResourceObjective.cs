using UnityEngine;
[System.Serializable]
public class CollectSpecificResourceObjective : SublevelObjectiveBase
{
    public ResourceData resource;
    public int resourcesNeeded;
    private int resourcesCollected;
    public override Sprite GetIcon() => resource.icon;

    public override void Initialize()
    {
        resourcesCollected = 0;
        goal = resourcesNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is CollectResourceEvent resourceEvent)
        {
            if (resourceEvent.resData == resource)
            {
                resourcesCollected += resourceEvent.amount;
            }
        }
        current = resourcesCollected;
    }

    public override bool isCompleted => resourcesCollected >= resourcesNeeded;
    public override float progress => (float)resourcesCollected / resourcesNeeded;
}
