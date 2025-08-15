using PrimeTween;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HeadDmg_Fire : Enemy, IDamagableEnemy
{

    [Header("COMPONENTES")]
    public BoxCollider attackCollider;
    public ParticleSystem[] rotationParticles;
    public Transform originRotation;
    public Animator animHead;

    [Header("VARIABLES ROTACION")]
    public float rotationAngle = 360f;
    public float rotationDuration = 6f;
    public float waitDuration = 1.5f;

    private Coroutine currentRotationCoroutine;
    private bool isBehaviorActive = false;

    [Header("VARIABLES DANO")]
    //public DamageBlock dmgBlock;
    public float damageCooldown = 2f;

    // Referencia a la interfaz del enemigo da�able
    private IDamagableEnemy _damagableEnemy;

    void Start()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
        foreach (ParticleSystem ps in rotationParticles)
        {
            if (ps != null) ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        StartEnemyBehavior();
    }

    //Solo debug////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) { OnHit(1); }
    }
    ///////////////////////////

    public void OnHit(int damageTaken)
    {
        /*life -= damageTaken; // Usa 'damageTaken' aqu�

        // Si la vida llega a 0, pedimos al componente que maneja la muerte que muera
        if (life <= 0)
        {
            if (_damagableEnemy != null)
            {
                _damagableEnemy.Die(); // Llama al m�todo Die() a trav�s de la interfaz
            }
            else
            {
                // Si no hay un componente que maneje la muerte, destruye directamente
                Debug.LogWarning($"EnemyDamage: {gameObject.name} muri�, pero no hay un IDamagableEnemy para manejarlo. Destruyendo directamente.", this);
            }
        }
        // Logica de recibir un golpe
        Debug.Log("HIT ENEMY HEAD");
        Debug.Log("Damage: " + damageTaken);*/
    }

    public void Die()
    {
        // ... toda la l�gica de lo que sucede cuando el enemigo muere ...
    }

    public void StartEnemyBehavior()
    {
        if (!isBehaviorActive)
        {
            isBehaviorActive = true;
            currentRotationCoroutine = StartCoroutine(RotationCycle());
            //Debug.Log("Enemy Behavior Started.");
        }
    }

    public void StopEnemyBehavior()
    {
        if (isBehaviorActive)
        {
            isBehaviorActive = false;
            if (currentRotationCoroutine != null)
            {
                StopCoroutine(currentRotationCoroutine);
                currentRotationCoroutine = null;
            }
            ActivateAttackComponents(false);
            //Debug.Log("Enemy Behavior Stopped.");
        }
    }

    IEnumerator RotationCycle()
    {
        while (isBehaviorActive)
        {
            ActivateAttackComponents(true);

            Quaternion initialRotation = transform.rotation;
            float targetAngle = initialRotation.eulerAngles.y + rotationAngle;

            float timer = 0f;
            while (timer < rotationDuration)
            {
                if (!isBehaviorActive) break;
                float currentYAngle = Mathf.Lerp(initialRotation.eulerAngles.y, targetAngle, timer / rotationDuration);
                originRotation.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, currentYAngle, initialRotation.eulerAngles.z);

                timer += Time.deltaTime;
                yield return null;
            }

            if (isBehaviorActive)
            {
                transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, targetAngle, initialRotation.eulerAngles.z);
            }

            if (isBehaviorActive) ActivateAttackComponents(false);

            if (isBehaviorActive) yield return new WaitForSeconds(waitDuration);
        }

        ActivateAttackComponents(false);
    }

    void ActivateAttackComponents(bool activate)
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = activate;
        }

        foreach (ParticleSystem ps in rotationParticles)
        {
            if (ps != null)
            {
                var emissionModule = ps.emission;

                if (activate)
                {
                    if (!ps.isPlaying)
                    {
                        ps.Play();
                    }
                    emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(100);
                }
                else
                {
                    emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(0);
                }
            }
        }

        animHead.SetBool("attacking", activate);

    }
}