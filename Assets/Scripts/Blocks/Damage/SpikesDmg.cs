using UnityEngine;
using System.Collections;

public class SpikesDmg : MonoBehaviour
{
    [Header("CONFIG MOVIMIENTO")]
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float movementDuration = 1.0f;
    public float timeOut = 2.0f;
    public float timeIn = 3.0f;
    public BoxCollider damageCollider;

    [Header("VARIABLES DA�O")]
    public float damageCooldown = 2f;
    private float lastDamageTime = -Mathf.Infinity;

    private DamageEffect dmgEffect => GetComponentInParent<DamageEffect>();

    private Coroutine spikeCycleCoroutine;
    private bool isMoving = false; // Condici�n para evitar movimientos duplicados

    void Start()
    {
        transform.localPosition = startPosition;

        if (damageCollider != null)
        {
            damageCollider.enabled = false;
        }

        StartSpikeCycle();
    }
    
    public void StartSpikeCycle()
    {
        if (!isMoving)
        {
            isMoving = true;
            spikeCycleCoroutine = StartCoroutine(SpikeMovementCycle());
            //Debug.Log("Spike cycle started.");
        }
    }
    public void StopSpikeCycle()
    {
        if (isMoving)
        {
            isMoving = false;
            if (spikeCycleCoroutine != null)
            {
                StopCoroutine(spikeCycleCoroutine);
                spikeCycleCoroutine = null;
            }
            if (damageCollider != null)
            {
                damageCollider.enabled = false;
            }
            transform.localPosition = startPosition;
            Debug.Log("Spike cycle stopped.");
        }
    }

    IEnumerator SpikeMovementCycle()
    {
        while (isMoving)
        {
            // 1. Mover las p�as hacia afuera (hacia endPosition)
            yield return StartCoroutine(MoveSpikes(startPosition, endPosition, movementDuration, true));
            //SoundManager.PlaySound(SFXType.SPIKEDAMAGE, transform.position, 0.2f);

            doDamage();

            // 2. Esperar un tiempo mientras las p�as est�n afuera
            if (!isMoving) break; // Si se detuvo durante el movimiento, salir
            yield return new WaitForSeconds(timeOut);

            doDamage();

            // 3. Mover las p�as hacia adentro (hacia startPosition)
            if (!isMoving) break; // Si se detuvo durante la espera, salir
            yield return StartCoroutine(MoveSpikes(endPosition, startPosition, movementDuration, false));



            // 4. Esperar un tiempo mientras las p�as est�n adentro
            if (!isMoving) break; // Si se detuvo durante el movimiento, salir
            yield return new WaitForSeconds(timeIn);
        }

        if (damageCollider != null)
        {
            damageCollider.enabled = false;
        }
        transform.localPosition = startPosition;
    }

    IEnumerator MoveSpikes(Vector3 startPos, Vector3 endPos, float duration, bool enableCollider)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (!isMoving) yield break; // Si el ciclo principal se detiene, detener esta coroutine tambi�n

            // Interpolar la posici�n de las p�as
            transform.localPosition = Vector3.Lerp(startPos, endPos, timer / duration);
            timer += Time.deltaTime;
            yield return null; // Esperar al siguiente frame
        }

        if (isMoving) 
        {
            transform.localPosition = endPos;

            if (damageCollider != null)
            {
                damageCollider.enabled = enableCollider;
            }
        }
    }

    private void doDamage()
    {
        dmgEffect.Activate();
    }

    // --- Dibujar Gizmos para ayudar a visualizar en el editor ---
    void OnDrawGizmosSelected()
    {
        // Solo dibuja gizmos si estamos en el editor y el script est� seleccionado
        if (Application.isEditor && !Application.isPlaying)
        {
            // Dibuja una esfera en la posici�n de inicio
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.parent != null ? transform.parent.TransformPoint(startPosition) : startPosition, 0.1f);
            Gizmos.DrawWireSphere(transform.parent != null ? transform.parent.TransformPoint(startPosition) : startPosition, 0.11f);
            // Dibuja una esfera en la posici�n final
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.parent != null ? transform.parent.TransformPoint(endPosition) : endPosition, 0.1f);
            Gizmos.DrawWireSphere(transform.parent != null ? transform.parent.TransformPoint(endPosition) : endPosition, 0.11f);

            // Dibuja una l�nea entre ellas
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.parent != null ? transform.parent.TransformPoint(startPosition) : startPosition,
                            transform.parent != null ? transform.parent.TransformPoint(endPosition) : endPosition);

            // Dibuja la posici�n actual del GameObject
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
        }
    }
}
