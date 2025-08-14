using UnityEngine;
using System.Collections;
using System;

public class TimedSpawnBehaviour : MonoBehaviour, IBlockBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    public float spawnInterval;

    public void StartBehaviour()
    {
        Debug.Log("START SPAWN");
        StartCoroutine(StartTimer());
    }

    public void StopBehaviour()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(spawnInterval);
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefabToSpawn, spawnPoint);
        StartCoroutine(StartTimer());
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }
}
