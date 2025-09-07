using UnityEngine;

public class EnemySetup : MonoBehaviour, IBlockSetup
{
    public EnemyDatabase enemyDatabase;
    [SerializeField] private string enemyID;
    [SerializeField] private Transform spawnPoint;

    public void SetupVariant(string _variant, MapContext _context)
    {
        var _enemyData = enemyDatabase.GetEnemyData(enemyID);
        if (_enemyData != null)
        {
            //GetComponent<EnemyBehaviour>().SetupBlock(_enemyData, spawnPoint);
        }
    }
}
