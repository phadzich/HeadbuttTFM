using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TimedSpawnBehaviour : MonoBehaviour, IBlockBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public bool randomStartDelay;
    public float randomStartDelayRange;

    public float spawnInterval;
    [SerializeField] private List<HealthEBehaviour> currEnemiesSpawned = new List<HealthEBehaviour>();
    [SerializeField] private int enemyLimit;

    private bool canSpawn => currEnemiesSpawned.Count < enemyLimit;

    public void StartBehaviour()
    {
        enemyLimit = LevelManager.Instance.currentContext.sublevel.config.limitPerSpawn;

        if (randomStartDelay)
        {
            float _delay = UnityEngine.Random.Range(0, randomStartDelayRange);
            StartCoroutine(DelayTimer(_delay));
        }
        else
        {
            StartCoroutine(StartTimer());
        }
    }

    public void StopBehaviour()
    {
        StopAllCoroutines();
    }

    private IEnumerator DelayTimer(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer() {
        yield return new WaitForSeconds(spawnInterval);
        Spawn();
    }

    private void Spawn()
    {
        if (!canSpawn) return;

        GameObject _enemy = Instantiate(prefabToSpawn, spawnPoint);

        HealthEBehaviour health = _enemy.GetComponent<HealthEBehaviour>();

        // Suscribimos al evento OnDeath
        health.OnDeath += HandleEnemyDeath;

        currEnemiesSpawned.Add(health);
        StartCoroutine(StartTimer());
    }

    private void HandleEnemyDeath(HealthEBehaviour enemy)
    {
        currEnemiesSpawned.Remove(enemy);
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }
}
