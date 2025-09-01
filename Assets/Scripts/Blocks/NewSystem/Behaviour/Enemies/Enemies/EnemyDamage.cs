using Unity.Cinemachine;
using UnityEngine;
using System.Linq;

public class EnemyDamage : Enemy
{
    public damageType typeOfDamage;
    public int damage;
    public bool breaksCombo;    

    public ParticleSystem damageParticles;
    public CinemachineImpulseSource impulseSource;

    [Header("KNOCKBACK")]
    public Vector3 currentPos;
    public Vector3 newDirection;

    [Header("SFX")]
    public AudioClip damageSound;
    public AudioSource audioSource;

    [Header("VARIABLES DANO")]
    public float damageCooldown = 1f;
    private float lastDamageTime = -Mathf.Infinity;

    // Referencia a la interfaz del enemigo da�able
    private IDamagableEnemy _damagableEnemy;

    private void Awake()
    {
        // Buscar el componente que implementa IDamagableEnemy en este mismo GameObject
        _damagableEnemy = GetComponent<IDamagableEnemy>();

        if (_damagableEnemy == null)
        {
            Debug.LogWarning($"EnemyDamage: No se encontr� un componente que implemente IDamagableEnemy en {gameObject.name}. Este enemigo no podr� morir por da�o.", this);
        }
    }
    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
    }
    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    private void PlayDamageSound()
    {
        audioSource.PlayOneShot(damageSound, 0.7f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Contacto con Fantasma!");
            float time = Time.time;
            if (Time.time - lastDamageTime >= damageCooldown) DoDamage();
        }
        lastDamageTime = Time.time;
    }

    //Solo debug////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) { OnHit(1); }
    }
    ////////////////////////////


    public void DoDamage()
    {
        //Debug.Log($"DAMAGED {typeOfDamage}:{damage}");
        if (typeOfDamage == damageType.Durability)
        {
            PlayerManager.Instance.playerEffects.TakeDamage(damage);
        }
        ScreenShake();
        PlayDamageSound();
        damageParticles.Play();
    }
    public void OnHit(int damageTaken) // Cambiado el nombre de la variable para evitar conflicto con la variable 'damage' de la clase
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
                Destroy(gameObject);
            }
        }

        Debug.Log("HIT ENEMY GHOST");
        Debug.Log("Damage Taken: " + damageTaken);*/
    }
    public enum damageType
    {
        Resources,
        Durability,
        Headbutts
    }

}
