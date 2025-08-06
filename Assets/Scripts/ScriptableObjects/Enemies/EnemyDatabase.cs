using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "GameData/Enemy/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemies;

    public EnemyData GetEnemyData(string id)
    {
        return enemies.Find(e => e.id == id);
    }
}