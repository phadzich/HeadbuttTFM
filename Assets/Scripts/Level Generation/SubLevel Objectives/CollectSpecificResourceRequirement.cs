using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CollectSpecificResourceRequirement : RequirementBase
{
    public ResourceData resource;
    public int resourcesNeeded;
    private int resourcesCollected;
    public override Sprite GetIcon() => resource.icon;

    public override void Initialize()
    {
        resourcesCollected = 0;
        goal = resourcesNeeded;
        current = 0;
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