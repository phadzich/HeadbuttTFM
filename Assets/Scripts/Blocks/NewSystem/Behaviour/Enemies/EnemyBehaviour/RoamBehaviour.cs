using UnityEngine;
using UnityEngine.AI;

public class RoamBehaviour : MonoBehaviour, IEnemyBehaviour
{
    [Header("Target & Movement")]
    public float moveSpeed;
    public float stoppingDistance;
    public float radius;

    [Header("NavMesh Agent Settings")]
    public float pathRecalculateFrequency;

    private NavMeshAgent agent;
    private float nextRecalculateTime;

    void Awake()
    {
        // Obtener el componente NavMeshAgent adjunto a este GameObject
        SetUpAgent();
    }

    private void SetUpAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("GhostEnemy: NavMeshAgent component missing en este GameObject! El fantasma no podra moverse.", this);
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
        SetUpTarget();
        nextRecalculateTime = Time.time + pathRecalculateFrequency;
    }

    void Update()
    {
        // Recalcular el destino del agente periodicamente
        RecalculateDestination();
    }

    private void RecalculateDestination()
    {
        // Si llegó a destino, elige otro
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetUpTarget();
        }

        // O si ya pasó el tiempo de recálculo, elige otro destino random
        if (Time.time >= nextRecalculateTime)
        {
            SetUpTarget();
            nextRecalculateTime = Time.time + pathRecalculateFrequency;
        }
    }

    // Metodo para establecer el destino del NavMeshAgent (el jugador)
    private void SetUpTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
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