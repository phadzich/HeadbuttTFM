using System;
using UnityEngine;

[RequireComponent(typeof(EnemySetup))]
public class EnemyBehaviour : MonoBehaviour, IBlockBehaviour
{
    EnemyData currentEnemy;

    public void SetupBlock(EnemyData _enenemyData, Transform _spawnPoint)
    {
        currentEnemy = _enenemyData;
        Instantiate(currentEnemy.prefab, _spawnPoint.position, Quaternion.identity, transform);
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }

}
