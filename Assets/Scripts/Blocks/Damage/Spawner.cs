using UnityEngine;
using System.Collections; // Necesario para Corrutinas

public class Spawner : MonoBehaviour
{
    [Header("GAMEOBJECT A INSTANCIAR")]
    // --- Configuración del Objeto a Spawnear ---
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public ParticleSystem spawnParticles;

    // Referencia a la instancia actualmente activa que este spawner ha creado.
    // Solo puede haber una instancia activa a la vez bajo esta condición.
    private GameObject _currentActiveInstance = null; // <-- NUEVA VARIABLE

    // --- Configuración de Condición de Activación ---
    public enum ActivationCondition
    {
        OnStart,            // Spawnea una vez al inicio del juego.
        OnPlayerTrigger,    // Spawnea cuando el jugador entra en su collider de trigger.
        AfterDelayOnce,     // Spawnea una vez después de un retardo.
        TimedInterval       // Spawnea repetidamente a intervalos.
    }

    [Header("PARÁMETROS ESPECÍFICOS")]
    public ActivationCondition activationCondition = ActivationCondition.OnPlayerTrigger;
    public string playerTriggerTag = "Player";
    public bool triggerOnce = true;
    private bool _hasTriggeredOnce = false; // Estado interno para triggerOnce
    public float triggerCooldownTime = 1.0f; // Tiempo que dura el cooldown
    public int maxTriggerSpawnsPerWindow = 1; // Por defecto 1 (un spawn por cooldown)
    private int _currentSpawnsInWindow = 0; // Contador de spawns dentro de la ventana actual
    private bool _isTriggerCooldownActive = false; // Bandera para controlar el cooldown
    public float initialDelay = 0f;
    public float repeatInterval = 3f;
    public int maxSpawns = 0;
    private int _currentSpawns = 0; // Contador interno para maxSpawns
    private Coroutine _spawnCoroutine; // Referencia a la corrutina para poder detenerla

    [Header("CONDICIÓN EXTRA: Una sola instancia a la vez")] // <-- NUEVA SECCIÓN EN EL INSPECTOR
    [Tooltip("Si es verdadero, el spawner solo instanciará un nuevo objeto si la instancia anterior (creada por este spawner) ha sido destruida.")]
    public bool allowOnlyOneActiveInstance = false; // <-- NUEVA VARIABLE BOOLEANA

    void Awake()
    {
        if (spawnPoint == null)
        {
            spawnPoint = this.transform;
        }

        if (activationCondition == ActivationCondition.OnPlayerTrigger)
        {
            Collider col = GetComponent<Collider>();
            if (col == null || !col.isTrigger)
            {
                Debug.LogWarning($"UnifiedSpawner on {gameObject.name}: 'OnPlayerTrigger' requires a 'Trigger Collider' attached to this GameObject.", this);
            }
        }
    }

    void Start()
    {
        if (activationCondition == ActivationCondition.OnStart ||
            activationCondition == ActivationCondition.AfterDelayOnce ||
            activationCondition == ActivationCondition.TimedInterval)
        {
            _spawnCoroutine = StartCoroutine(HandleTimedActivation());
        }
    }

    void OnDestroy()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }
        // Opcional: Si el spawner es destruido y _currentActiveInstance es suyo,
        // puedes decidir destruirla también, pero es mejor que el objeto se gestione solo.
        // if (_currentActiveInstance != null && allowOnlyOneActiveInstance)
        // {
        //     Destroy(_currentActiveInstance); 
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        if (activationCondition == ActivationCondition.OnPlayerTrigger)
        {
            // Primero, verifica si el objeto que entró es el jugador.
            if (!other.CompareTag(playerTriggerTag))
            {
                return;
            }

            // Si es de un solo uso y ya se activó, ignora.
            if (triggerOnce && _hasTriggeredOnce)
            {
                return;
            }

            // --- NUEVA LÓGICA: Verificar si ya hay una instancia activa ---
            if (allowOnlyOneActiveInstance && _currentActiveInstance != null)
            {
                Debug.Log($"UnifiedSpawner '{gameObject.name}': No se puede spawnear '{prefabToSpawn.name}'. Ya existe una instancia activa.", this);
                return; // No spawnea si ya hay uno activo
            }

            // Si el cooldown está activo, verificamos si podemos spawnear más dentro de la ventana.
            if (_isTriggerCooldownActive)
            {
                if (maxTriggerSpawnsPerWindow == 0 || _currentSpawnsInWindow < maxTriggerSpawnsPerWindow)
                {
                    PerformSpawn();
                    _currentSpawnsInWindow++;
                }
                return;
            }

            PerformSpawn();
            _currentSpawnsInWindow = 1; // Reiniciamos el contador para la nueva ventana.
            _isTriggerCooldownActive = true; // Activamos el cooldown.
            StartCoroutine(TriggerCooldownRoutine()); // Iniciamos la corrutina del cooldown.

            if (triggerOnce)
            {
                _hasTriggeredOnce = true;
                // gameObject.SetActive(false); // Desactivar el spawner después de un solo uso completo
            }
        }
    }

    private IEnumerator TriggerCooldownRoutine()
    {
        yield return new WaitForSeconds(triggerCooldownTime);
        _isTriggerCooldownActive = false; // Desactiva el cooldown
        _currentSpawnsInWindow = 0; // Reinicia el contador para la próxima ventana
    }

    private IEnumerator HandleTimedActivation()
    {
        // Espera el retardo inicial antes del primer spawn.
        if (initialDelay > 0)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        // Bucle principal para spawns repetidos o un único spawn después del retardo.
        while (true)
        {
            // Condición de salida si se alcanzó el límite de spawns.
            if (maxSpawns != 0 && _currentSpawns >= maxSpawns)
            {
                Debug.Log($"UnifiedSpawner '{gameObject.name}': Límite máximo de {maxSpawns} spawns alcanzado. Deteniendo spawns cronometrados.", this);
                yield break; // Sale de la corrutina.
            }

            // --- NUEVA LÓGICA: Verificar si ya hay una instancia activa ---
            if (allowOnlyOneActiveInstance && _currentActiveInstance != null)
            {
                Debug.Log($"UnifiedSpawner '{gameObject.name}': No se puede spawnear '{prefabToSpawn.name}'. Ya existe una instancia activa. Esperando...", this);
                yield return new WaitForSeconds(repeatInterval); // Espera antes de reintentar
                continue; // Vuelve al inicio del bucle para reevaluar la condición
            }

            PerformSpawn(); // Realiza el spawn.
            _currentSpawns++; // Incrementa el contador.

            // Si la condición es solo un spawn al inicio o después de un retardo único, sal de la corrutina.
            if (activationCondition == ActivationCondition.AfterDelayOnce || activationCondition == ActivationCondition.OnStart)
            {
                yield break;
            }

            // Para TimedInterval, espera el siguiente intervalo antes de la próxima iteración.
            yield return new WaitForSeconds(repeatInterval);
        }
    }

    private void PerformSpawn()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"UnifiedSpawner on {gameObject.name}: No 'Prefab To Spawn' assigned. Cannot spawn.", this);
            return;
        }

        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        spawnParticles.Play();

        // --- Almacenar la referencia a la nueva instancia si 'allowOnlyOneActiveInstance' es true ---
        if (allowOnlyOneActiveInstance)
        {
            _currentActiveInstance = spawnedObject;
            // Intentar que el objeto spawneado notifique al spawner cuando sea destruido.
            // Para que esto funcione, el objeto spawneado necesitará un script que implemente ISpawnable.
            ISpawnable spawnerLink = spawnedObject.GetComponent<ISpawnable>();
            if (spawnerLink != null)
            {
                spawnerLink.SetSpawner(this);
            }
            else
            {
                Debug.LogWarning($"UnifiedSpawner '{gameObject.name}': Objeto '{spawnedObject.name}' instanciado, pero no tiene un componente que implemente 'Spawner.ISpawnable'. 'allowOnlyOneActiveInstance' puede no funcionar correctamente si el objeto no notifica su destrucción.", spawnedObject);
            }
        }

        Debug.Log($"[UnifiedSpawner] Spawned: {spawnedObject.name} at {spawnPoint.position}. Its own scripts will dictate its behavior.");
    }

    /// <summary>
    /// Método público para que los objetos instanciados notifiquen al spawner cuando son destruidos.
    /// Esto es crucial para la funcionalidad 'allowOnlyOneActiveInstance'.
    /// </summary>
    /// <param name="destroyedInstance">La instancia de GameObject que ha sido destruida.</param>
    public void NotifySpawnedObjectDestroyed(GameObject destroyedInstance) // <-- NUEVO MÉTODO
    {
        // Solo limpia la referencia si el objeto destruido es el que actualmente está registrado.
        if (allowOnlyOneActiveInstance && _currentActiveInstance == destroyedInstance)
        {
            _currentActiveInstance = null; // Borra la referencia.
            Debug.Log($"UnifiedSpawner '{gameObject.name}': Instancia '{destroyedInstance.name}' desregistrada. Spawner listo para crear una nueva (si 'allowOnlyOneActiveInstance' está activo).", this);
        }
        else if (allowOnlyOneActiveInstance && _currentActiveInstance != null && _currentActiveInstance != destroyedInstance)
        {
            Debug.LogWarning($"UnifiedSpawner '{gameObject.name}': Se intentó desregistrar '{destroyedInstance.name}', pero la instancia activa registrada es '{_currentActiveInstance.name}'. Ignorando.", destroyedInstance);
        }
        else if (allowOnlyOneActiveInstance && _currentActiveInstance == null)
        {
            // Esto podría ocurrir si el objeto ya fue desregistrado o no era la instancia que el spawner estaba esperando.
            Debug.LogWarning($"UnifiedSpawner '{gameObject.name}': Se intentó desregistrar '{destroyedInstance.name}', pero no había ninguna instancia activa registrada. Ignorando.", destroyedInstance);
        }
    }

    /// <summary>
    /// Interfaz que los scripts de los prefabs instanciados deben implementar
    /// si necesitan notificar al Spawner de su destrucción.
    /// </summary>
    public interface ISpawnable // <-- NUEVA INTERFAZ
    {
        void SetSpawner(Spawner spawner); // Método para que el spawner se asigne al objeto instanciado.
    }
}