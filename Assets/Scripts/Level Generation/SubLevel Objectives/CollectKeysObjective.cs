using UnityEngine;
[System.Serializable]
public class CollectKeysObjective : SublevelObjectiveBase
{
    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;

    public int keysNeeded;
    private int keysCollected;

    public override void Initialize()
    {
        keysCollected = 0;
        goal = keysNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is CollectKeyEvent)
            keysCollected++;
        current = keysCollected;
        //Debug.Log($"{current}/{goal}");
    }

    public override bool isCompleted => keysCollected >= keysNeeded;
    public override float progress => (float)keysCollected / keysNeeded;
}