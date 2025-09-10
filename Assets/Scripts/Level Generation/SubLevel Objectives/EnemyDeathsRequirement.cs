using UnityEngine;

[System.Serializable]
public class EnemyDeathsRequirement : RequirementBase
{
    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;

    public int enemiesNeeded;

    public override void Initialize()
    {
        goal = enemiesNeeded;
        current = 0;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is EnemyDeathsEvent enemyDeathsEvent)
        {
            current ++;
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}