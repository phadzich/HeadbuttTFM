using UnityEngine;

[System.Serializable]
public class CollectSpecificResourceRequirement : RequirementBase
{
    public ResourceData resource;
    public int resourcesNeeded;
    private int resourcesCollected;

    public override void Initialize(int _id)
    {
        
        targetId = _id;
        resourcesCollected = 0;
        goal = resourcesNeeded;
        //Debug.Log("INIT");
        //Debug.Log(gateId);
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
        //Debug.Log($"{current}/{goal}");
        //Debug.Log($"{gateId}:{isCompleted}");
    }

    public override bool isCompleted => resourcesCollected >= resourcesNeeded;
    public override float progress => (float)resourcesCollected / resourcesNeeded;
}