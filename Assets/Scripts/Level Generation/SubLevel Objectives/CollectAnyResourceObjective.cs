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
        current = 0;
        goal = resourcesNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is CollectResourceEvent)
            current++;
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}
