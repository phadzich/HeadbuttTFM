using UnityEngine;
using UnityEngine.AI;

public class FollowEBheaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("Target & Movement")]
    public Transform target;
    public float moveSpeed;
    public float stoppingDistance;

    [Header("NavMesh Agent Settings")]
    public float pathRecalculateFrequency;

    // Referencia al componente NavMeshAgent
    private NavMeshAgent agent;
    // Para controlar la frecuencia de recalculado del camino
    private float nextRecalculateTime;

    void Awake()
    {
        // Obtener el componente NavMeshAgent adjunto a este GameObject
        SetUpAgent();

        // Buscar al jugador si la referencia no est� asignada en el Inspector
        SetUpTarget();
    }

    private void SetUpTarget()
    {
        if (target == null)
        {
            GameObject playerObject = PlayerManager.Instance.playerStates.gameObject;
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

    private void SetUpAgent()
    {
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
    }

    void Start()
    {
        // Realizar la primera solicitud de destino inmediatamente
        // Esto asume que el NavMesh ya ha sido horneado/generado cuando este Start() se ejecuta.
        SetAgentDestination();
        nextRecalculateTime = Time.time + pathRecalculateFrequency;
    }

    void Update()
    {
        // Recalcular el destino del agente periodicamente
        RecalculateDestination();
    }

    private void RecalculateDestination()
    {
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
        }
        else if (!agent.isOnNavMesh)
        {
            // �til para depurar si el fantasma no logra encontrar un NavMesh al inicio
            Debug.LogWarning($"GhostEnemy: {gameObject.name} no est� en un NavMesh. No se puede establecer destino.");
        }
    }


    public void OnHit()
    {
        
    }

    public void StartBehaviour()
    {
        
    }

    public void StopBehaviour()
    {
        
    }
}
