using PrimeTween;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HeadDmg_Fire : MonoBehaviour
{
    [Header("COMPONENTES")]
    public BoxCollider attackCollider;
    public ParticleSystem[] rotationParticles;
    public Animator animHead;

    [Header("VARIABLES ROTACION")]
    public float rotationAngle = 360f;
    public float rotationDuration = 6f;
    public float waitDuration = 1.5f;

    private Coroutine currentRotationCoroutine;
    private bool isBehaviorActive = false;

    [Header("VARIABLES DAÑO")]
    public DamageBlock dmgBlock;
    public float damageCooldown = 2f;
    private float lastDamageTime = -Mathf.Infinity;

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

    public void StartEnemyBehavior()
    {
        if (!isBehaviorActive)
        {
            isBehaviorActive = true;
            currentRotationCoroutine = StartCoroutine(RotationCycle());
            Debug.Log("Enemy Behavior Started.");
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
            Debug.Log("Enemy Behavior Stopped.");
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
                transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, currentYAngle, initialRotation.eulerAngles.z);

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

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Contacto!");
            float time = Time.time;
            if (Time.time - lastDamageTime >= damageCooldown)
                if (HelmetManager.Instance.currentHelmet.helmetEffect != EffectTypeEnum.LavaBoost)
                {
                    dmgBlock.DoDamage();
                }
        }
        lastDamageTime = Time.time;
    }
}

