using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEBehaviour : MonoBehaviour, IEnemyBehaviour, IElementReactive
{

    public Transform axis;
    public GameObject projectilePrefab;

    public float shootingInterval;

    public float speedMultiplier;
    public float intervalMultiplier;

    public int projectileCount;
    public float projectileSpeed;

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();
    public void StartBehaviour()
    {
        StartCoroutine(StartTimer());
    }

    public void StopBehaviour()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(shootingInterval*intervalMultiplier);
        Shoot();
    }

    public void Shoot()
    {
        float angleStep = 360f / projectileCount;
        float angle = 0f;

        for (int i = 0; i < projectileCount; i++)
        {
            // Dirección en base al ángulo
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirZ = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, 0, dirZ);

            // Instanciar bala
            GameObject projectile = Instantiate(projectilePrefab, axis.position, Quaternion.LookRotation(dir));

            // Darle velocidad
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = dir * projectileSpeed*speedMultiplier;
            }

            // Avanzar ángulo
            angle += angleStep;
        }

        StartCoroutine(StartTimer());
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }

    public void OnHit()
    {
        
    }

    public bool IsAllowedForSource(InteractionSource source)
    {
        return AllowedSources.Count == 0 || AllowedSources.Contains(source);
    }

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
        Debug.Log("checking interactions");
        if (targetElement == ElementType.Grass)
        {
            switch (sourceElement)
            {
                case ElementType.Water:
                    intervalMultiplier = .5f;
                    speedMultiplier = 2;
                    Debug.Log("SHOOT FRENZY");
                    break;
                case ElementType.Fire:
                    intervalMultiplier = 2;
                    speedMultiplier = .5f;
                    Debug.Log("BURN SLOW");
                    break;
            }
        }
    }
}
