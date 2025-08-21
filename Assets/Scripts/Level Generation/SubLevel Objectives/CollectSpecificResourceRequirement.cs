[System.Serializable]
public class CollectSpecificResourceRequirement : GateRequirementBase
{
    public ResourceData resource;
    public int resourcesNeeded;
    private int resourcesCollected;

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