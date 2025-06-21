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

    [Header("VARIABLES DAÑO")]
    public DamageBlock dmgBlock;
    public float damageCooldown = 2f;
    private float lastDamageTime = -Mathf.Infinity;

    private Coroutine spikeCycleCoroutine;
    private bool isMoving = false; // Condición para evitar movimientos duplicados

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
            Debug.Log("Spike cycle started.");
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
            // 1. Mover las púas hacia afuera (hacia endPosition)
            yield return StartCoroutine(MoveSpikes(startPosition, endPosition, movementDuration, true));

            // 2. Esperar un tiempo mientras las púas están afuera
            if (!isMoving) break; // Si se detuvo durante el movimiento, salir
            yield return new WaitForSeconds(timeOut);

            // 3. Mover las púas hacia adentro (hacia startPosition)
            if (!isMoving) break; // Si se detuvo durante la espera, salir
            yield return StartCoroutine(MoveSpikes(endPosition, startPosition, movementDuration, false));

            // 4. Esperar un tiempo mientras las púas están adentro
            if (!isMoving) break; // Si se detuvo durante el movimiento, salir
            yield return new WaitForSeconds(timeIn);
        }

        if (damageCollider != null)
        {
            damageCollider.enabled = false;
        }

        transform.localPosition = startPosition;
    }

    /*/// <summary>
    /// Coroutine auxiliar para mover las púas entre dos posiciones.
    /// </summary>
    /// <param name="startPos">Posición inicial del movimiento.</param>
    /// <param name="endPos">Posición final del movimiento.</param>
    /// <param name="duration">Duración del movimiento.</param>
    /// <param name="enableCollider">Si el collider debe activarse al final de este movimiento.</param>*/
    IEnumerator MoveSpikes(Vector3 startPos, Vector3 endPos, float duration, bool enableCollider)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (!isMoving) yield break; // Si el ciclo principal se detiene, detener esta coroutine también

            // Interpolar la posición de las púas
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

    // --- Dibujar Gizmos para ayudar a visualizar en el editor ---
    void OnDrawGizmosSelected()
    {
        // Solo dibuja gizmos si estamos en el editor y el script está seleccionado
        if (Application.isEditor && !Application.isPlaying)
        {
            // Dibuja una esfera en la posición de inicio
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.parent != null ? transform.parent.TransformPoint(startPosition) : startPosition, 0.1f);
            Gizmos.DrawWireSphere(transform.parent != null ? transform.parent.TransformPoint(startPosition) : startPosition, 0.11f);
            // Dibuja una esfera en la posición final
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.parent != null ? transform.parent.TransformPoint(endPosition) : endPosition, 0.1f);
            Gizmos.DrawWireSphere(transform.parent != null ? transform.parent.TransformPoint(endPosition) : endPosition, 0.11f);

            // Dibuja una línea entre ellas
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.parent != null ? transform.parent.TransformPoint(startPosition) : startPosition,
                            transform.parent != null ? transform.parent.TransformPoint(endPosition) : endPosition);

            // Dibuja la posición actual del GameObject
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
        }
    }
}
