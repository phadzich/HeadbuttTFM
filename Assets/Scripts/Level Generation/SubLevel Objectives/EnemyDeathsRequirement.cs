using UnityEngine;

[System.Serializable]
public class EnemyDeathsRequirement : RequirementBase
{
    public int enemiesNeeded;

    public override void Initialize(int _id)
    {
        targetId = _id;
        goal = enemiesNeeded;
        current = 0;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is EnemyDeathsEvent enemyDeathsEvent)
        {
            current ++;
            Debug.Log($"ENEMY DEATHS {current}/{goal} completed {isCompleted}");
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}