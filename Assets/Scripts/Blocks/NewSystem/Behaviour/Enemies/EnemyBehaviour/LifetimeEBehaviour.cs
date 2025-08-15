using System.Collections.Generic;
using UnityEngine;

public class LifetimeEBehaviour : MonoBehaviour, IEnemyBehaviour
{
    public float lifetime;
    private float elapsedTime;
    private bool isAlive;

    private void StartTimer()
    {
        isAlive = true;
    }

    private void StopTimer()
    {
        isAlive = false;
    }

    private void Update()
    {
        if (isAlive)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > lifetime)
            {
                SelfDestruct();
            }
        }
    }

    private void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    public void StartBehaviour()
    {
        StartTimer();
    }

    public void StopBehaviour()
    {
        StopTimer();
    }

    public void OnHit()
    {
    }
}
