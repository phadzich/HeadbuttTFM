using UnityEngine;
using UnityEngine.AI; // ¡Importante! Necesitas este namespace para NavMeshAgent
using System.Collections;

public class GhostEnemy : MonoBehaviour
{
    [Header("Target & Movement")]
    [Tooltip("Referencia al Transform del jugador (se buscará automáticamente si es nula).")]
    public Transform target;
    [Tooltip("Velocidad de movimiento del fantasma. Esto se asigna al NavMeshAgent.")]
    public float moveSpeed = 3.5f;
    [Tooltip("Distancia mínima al objetivo para considerar que se ha llegado y recalcular (si aplica).")]
    public float stoppingDistance = 0.5f;

    [Header("NavMesh Agent Settings")]
    [Tooltip("Frecuencia con la que el fantasma recalcula su camino (en segundos). Menor valor = más reactivo, mayor costo.")]
    public float pathRecalculateFrequency = 0.5f;

    [Header("Lifetime")]
    [Tooltip("Tiempo en segundos antes de que el fantasma desaparezca automáticamente. Establece 0 para que el fantasma NO sea destruido.")]
    public float lifetime = 5.0f;

    // Referencia al componente NavMeshAgent
    private NavMeshAgent agent;
    // Para controlar la frecuencia de recalculado del camino
    private float nextRecalculateTime;

    // Referencia al Spawner que lo creó
    // Esto será asignado por el Spawner al instanciar el fantasma.
    [HideInInspector] // Oculta esta variable en el Inspector, ya que será asignada por código.
    public Spawner creatorSpawner;

    void Awake()
    {
        // Obtener el componente NavMeshAgent adjunto a este GameObject
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("GhostEnemy: ¡NavMeshAgent component missing en este GameObject! El fantasma no podrá moverse.", this);
            // Considera destruir el fantasma aquí si el NavMeshAgent es indispensable para su funcionamiento.
            // Destroy(gameObject); 
        }
        else
        {
            // Configurar propiedades iniciales del NavMeshAgent usando las variables del script
            agent.speed = moveSpeed;
            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = true;
            agent.updateUpAxis = false;
        }

        // Buscar al jugador si la referencia no está asignada en el Inspector
        if (target == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("GhostEnemy: Jugador (con tag 'Player') no encontrado en la escena. El fantasma no se moverá hacia un objetivo.", this);
            }
        }
    }

    void Start()
    {
        // Iniciar el temporizador de vida si lifetime es mayor que 0
        if (lifetime > 0)
        {
            StartCoroutine(DestroyAfterLifetime());
        }
        else
        {
            Debug.Log($"GhostEnemy: {gameObject.name} persistirá indefinidamente ya que su 'lifetime' es 0.");
        }

        // Realizar la primera solicitud de destino inmediatamente
        // Esto asume que el NavMesh ya ha sido horneado/generado cuando este Start() se ejecuta.
        SetAgentDestination();
        nextRecalculateTime = Time.time + pathRecalculateFrequency;
    }

    void Update()
    {
        // Si no hay agente, objetivo o el agente no está en un NavMesh, no hacer nada.
        if (agent == null || target == null || !agent.isOnNavMesh)
        {
            return;
        }

        // Recalcular el destino del agente periódicamente
        if (Time.time >= nextRecalculateTime)
        {
            SetAgentDestination();
            nextRecalculateTime = Time.time + pathRecalculateFrequency;
        }
    }

    // Método para establecer el destino del NavMeshAgent (el jugador)
    void SetAgentDestination()
    {
        if (target != null && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);

            // Opcional: Visualización del camino del agente en el editor (solo para depuración)
            if (agent.hasPath)
            {
                Color pathColor = Color.cyan;
                for (int i = 0; i < agent.path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], pathColor, pathRecalculateFrequency * 1.2f);
                }
            }
        }
        else if (!agent.isOnNavMesh)
        {
            // Útil para depurar si el fantasma no logra encontrar un NavMesh al inicio
            Debug.LogWarning($"GhostEnemy: {gameObject.name} no está en un NavMesh. No se puede establecer destino.");
        }
    }

    // Corrutina para destruir el fantasma después de su tiempo de vida
    private IEnumerator DestroyAfterLifetime()
    {
        if (lifetime <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(lifetime);

        // Notificar al spawner si existe antes de destruirse
        // ESTA ES LA LÍNEA ADICIONAL PARA EL REGISTRO DEL SPAWNER
        if (creatorSpawner != null)
        {
            creatorSpawner.NotifySpawnedObjectDestroyed(gameObject);
        }

        Destroy(gameObject);
        Debug.Log($"GhostEnemy: {gameObject.name} desapareció después de {lifetime} segundos.");
    }
}