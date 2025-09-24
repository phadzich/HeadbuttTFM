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

    [SerializeField] private ShootingMode shootingMode;
    [SerializeField] private ShootingDirection direction;

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    private EnemySFX sfx => GetComponent<EnemySFX>();


    public void SetUpShooter(ShootingMode _mode, ShootingDirection _direction)
    {
        shootingMode = _mode;
        direction = _direction;
    }

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
        if (sfx != null)
        {
            Debug.Log("PLAY SOUND SHOOTT");
            sfx.PlayAttack();
        }

        if (shootingMode == ShootingMode.Radial)
        {
            // Se divide 360 (una vuelta) entre la cantidad de proyectiles
            float angleStep = 360f / projectileCount;
            float angle = 0f;

            for (int i = 0; i < projectileCount; i++)
            {
                //Se calcula la direccion tomando en cuenta el angulo en el que debe spawnear
                float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
                float dirZ = Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 dir = new Vector3(dirX, 0, dirZ);

                SpawnProjectile(dir);

                // Se calcula el angulo de la siguiente bala
                angle += angleStep;
            }
        }
        else if (shootingMode == ShootingMode.Directional)
        {
            Vector3 dir = Vector3.zero;

            switch (direction)
            {
                case ShootingDirection.Front:
                    dir = axis.forward;
                    break;
                case ShootingDirection.Back:
                    dir = -axis.forward;
                    break;
                case ShootingDirection.Left:
                    dir = -axis.right;
                    break;
                case ShootingDirection.Right:
                    dir = axis.right;
                    break;
            }

            for (int i = 0; i < projectileCount; i++)
            {
                SpawnProjectile(dir);
            }
        }

        StartCoroutine(StartTimer());
    }

    private void SpawnProjectile(Vector3 dir)
    {
        GameObject projectile = Instantiate(projectilePrefab, axis.position, Quaternion.LookRotation(dir));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * projectileSpeed * speedMultiplier;
        }
    }

    public void OnHit()
    {}

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

public enum ShootingMode
{
    Radial,
    Directional
}

public enum ShootingDirection
{
    None,
    Front,
    Back,
    Left,
    Right
}
