using UnityEngine;
using UnityEngine.AI; // �Importante! Necesitas este namespace para NavMeshAgent
using System.Collections;

public class GhostEnemy : MonoBehaviour, IDamagableEnemy
{
    //Variables para detectar si el enemigo debe morir
    private bool isDying = false;

    [Header("Target & Movement")]
    [Tooltip("Referencia al Transform del jugador (se buscar� autom�ticamente si es nula).")]
    public Transform target;
    [Tooltip("Velocidad de movimiento del fantasma. Esto se asigna al NavMeshAgent.")]
    public float moveSpeed = 3.5f;
    [Tooltip("Distancia m�nima al objetivo para considerar que se ha llegado y recalcular (si aplica).")]
    public float stoppingDistance = 0.5f;

    [Header("NavMesh Agent Settings")]
    [Tooltip("Frecuencia con la que el fantasma recalcula su camino (en segundos). Menor valor = m�s reactivo, mayor costo.")]
    public float pathRecalculateFrequency = 0.5f;

    [Header("Lifetime")]
    [Tooltip("Tiempo en segundos antes de que el fantasma desaparezca autom�ticamente. Establece 0 para que el fantasma NO sea destruido.")]
    public float lifetime = 5.0f;

    // Referencia al componente NavMeshAgent
    private NavMeshAgent agent;
    // Para controlar la frecuencia de recalculado del camino
    private float nextRecalculateTime;

    // Referencia al Spawner que lo cre�
    // Esto ser� asignado por el Spawner al instanciar el fantasma.
    [HideInInspector] // Oculta esta variable en el Inspector, ya que ser� asignada por c�digo.
    public Spawner creatorSpawner;

    void Awake()
    {
        // Obtener el componente NavMeshAgent adjunto a este GameObject
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("GhostEnemy: �NavMeshAgent component missing en este GameObject! El fantasma no podr� moverse.", this);

        }
        else
        {
            // Configurar propiedades iniciales del NavMeshAgent usando las variables del script
            agent.speed = moveSpeed;
            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = true;
            agent.updateUpAxis = false;
        }

        // Buscar al jugador si la referencia no est� asignada en el Inspector
        if (target == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("GhostEnemy: Jugador (con tag 'Player') no encontrado en la escena. El fantasma no se mover� hacia un objetivo.", this);
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
            Debug.Log($"GhostEnemy: {gameObject.name} persistir� indefinidamente ya que su 'lifetime' es 0.");
        }

        // Realizar la primera solicitud de destino inmediatamente
        // Esto asume que el NavMesh ya ha sido horneado/generado cuando este Start() se ejecuta.
        SetAgentDestination();
        nextRecalculateTime = Time.time + pathRecalculateFrequency;
    }

    void Update()
    {
        // Si no hay agente, objetivo o el agente no est� en un NavMesh, no hacer nada.
        if (agent == null || target == null || !agent.isOnNavMesh)
        {
            return;
        }

        // Si el enemigo está muriendo, detener el movimiento del NavMeshAgent
        if (isDying && agent.enabled)
        {
            agent.isStopped = true; // Detiene el movimiento
            agent.enabled = false; // Deshabilita el componente NavMeshAgent si ya no lo usaremos
            return; // No recalcular ni mover si está muriendo
        }

        // Recalcular el destino del agente peri�dicamente
        if (Time.time >= nextRecalculateTime)
        {
            SetAgentDestination();
            nextRecalculateTime = Time.time + pathRecalculateFrequency;
        }
    }    

    // Metodo para establecer el destino del NavMeshAgent (el jugador)
    void SetAgentDestination()
    {
        if (target != null && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);

            // Opcional: Visualizaci�n del camino del agente en el editor (solo para depuraci�n)
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
            // �til para depurar si el fantasma no logra encontrar un NavMesh al inicio
            Debug.LogWarning($"GhostEnemy: {gameObject.name} no est� en un NavMesh. No se puede establecer destino.");
        }
    }

    // Implementación del método Die() de la interfaz IDamagableEnemy
    public void Die()
    {
        if (isDying) return; // Ya está muriendo, evita duplicados

        isDying = true; // Marca que está muriendo

        // Deshabilita cualquier comportamiento adicional aquí para que no siga actuando
        // Por ejemplo, deshabilita el colisionador, el NavMeshAgent, etc.
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false; // Deshabilita el componente para que no interfiera
        }
        // Puedes deshabilitar el Collider si tiene uno que no sea trigger para el daño
        // GetComponent<Collider>().enabled = false; 

        // Llama a la corrutina de destrucción, que también notificará al spawner
        StartCoroutine(DestroyAfterLifetime());
    }

    // Corrutina para destruir el fantasma después de su tiempo de vida O cuando es "Die"
    private IEnumerator DestroyAfterLifetime()
    {
        // Si el enemigo ya está marcado como muriendo por daño, no esperamos el lifetime.
        // Si no está muriendo, significa que estamos esperando el tiempo de vida.
        if (!isDying && lifetime > 0)
        {
            yield return new WaitForSeconds(lifetime);
        }
        // Si _isDying es true (llamado desde Die()), no hay espera adicional, o ya se esperó el lifetime.
        // Si lifetime es <= 0, no esperamos el tiempo de vida.

        // Notificar al spawner si existe antes de destruirse
        if (creatorSpawner != null)
        {
            creatorSpawner.NotifySpawnedObjectDestroyed(gameObject);
        }

        Destroy(gameObject);
        Debug.Log($"GhostEnemy: {gameObject.name} desapareció."); // Mensaje más generalizado
    }

}