using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SimpleTimedSpawnBehaviour : MonoBehaviour, IBlockBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public bool randomStartDelay;
    public float randomStartDelayRange;

    public float spawnInterval;

    public void StartBehaviour()
    {
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
        GameObject _enemy = Instantiate(prefabToSpawn, spawnPoint);
        StartCoroutine(StartTimer());
    }
    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }
}
