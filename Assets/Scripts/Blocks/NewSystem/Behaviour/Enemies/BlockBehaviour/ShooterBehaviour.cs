using UnityEngine;
using System.Collections;

public class ShooterBehaviour : MonoBehaviour, IBlockBehaviour
{

    public Transform axis;
    public GameObject projectilePrefab;

    public float shootingInterval;

    public int projectileCount;
    public float projectileSpeed;

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
        yield return new WaitForSeconds(shootingInterval);
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
                rb.linearVelocity = dir * projectileSpeed;
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
}
