using Unity.Cinemachine;
using UnityEngine;
using System.Linq;

public class EnemyDamage : Enemy
{
    public damageType typeOfDamage;
    public int damage;
    public EffectTypeEnum helmetCounter;
    public bool breaksCombo;    

    public ParticleSystem damageParticles;
    public CinemachineImpulseSource impulseSource;

    [Header("KNOCKBACK")]
    Block[] directions = new Block[4];
    public Vector3 currentPos;
    public Vector3 newDirection;

    [Header("SFX")]
    public AudioClip damageSound;
    public AudioSource audioSource;

    [Header("VARIABLES DANO")]
    public float damageCooldown = 1f;
    private float lastDamageTime = -Mathf.Infinity;

    // Referencia a la interfaz del enemigo dañable
    private IDamagableEnemy _damagableEnemy;

    private void Awake()
    {
        // Buscar el componente que implementa IDamagableEnemy en este mismo GameObject
        _damagableEnemy = GetComponent<IDamagableEnemy>();

        if (_damagableEnemy == null)
        {
            Debug.LogWarning($"EnemyDamage: No se encontró un componente que implemente IDamagableEnemy en {gameObject.name}. Este enemigo no podrá morir por daño.", this);
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

    public void PushPlayerRandomly()
    {

        directions[0] = PlayerManager.Instance.playerMovement.blockBelow.up;
        directions[1] = PlayerManager.Instance.playerMovement.blockBelow.down;
        directions[2] = PlayerManager.Instance.playerMovement.blockBelow.left;
        directions[3] = PlayerManager.Instance.playerMovement.blockBelow.right;
        directions = directions.OrderBy(d => Random.value).ToArray();

        foreach (Block dir in directions)
        {
            if (dir.isWalkable)
            {

                Vector2 delta = dir.sublevelPosition - PlayerManager.Instance.playerMovement.blockBelow.sublevelPosition;
                Vector2 currentPos = PlayerManager.Instance.playerMovement.blockBelow.sublevelPosition;
                newDirection = GetCardinalDirection(delta);
                //Debug.Log(dir);
                break;
            }
        }

        //Debug.Log(newDirection);
        PlayerManager.Instance.playerMovement.Knockback(newDirection);
    }
    Vector3 GetCardinalDirection(Vector2 delta)
    {
        // Elige el eje con mayor valor absoluto
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return new Vector3(Mathf.Sign(delta.x), 0, 0);
        }
        else if (Mathf.Abs(delta.y) > 0)
        {
            return new Vector3(0, 0, Mathf.Sign(delta.y));
        }
        else
        {
            // Si delta es cero en ambos ejes (caso raro), no moverse
            return Vector3.zero;
        }
    }

    public void DoDamage()
    {
        //Debug.Log($"DAMAGED {typeOfDamage}:{damage}");
        if (typeOfDamage == damageType.Durability)
        {
            HelmetManager.Instance.currentHelmet.TakeDamage(damage);
        }
        ScreenShake();
        PlayDamageSound();
        damageParticles.Play();
    }
    public override void OnHit(int damageTaken) // Cambiado el nombre de la variable para evitar conflicto con la variable 'damage' de la clase
    {
        life -= damageTaken; // Usa 'damageTaken' aquí

        // Si la vida llega a 0, pedimos al componente que maneja la muerte que muera
        if (life <= 0)
        {
            if (_damagableEnemy != null)
            {
                _damagableEnemy.Die(); // Llama al método Die() a través de la interfaz
            }
            else
            {
                // Si no hay un componente que maneje la muerte, destruye directamente
                Debug.LogWarning($"EnemyDamage: {gameObject.name} murió, pero no hay un IDamagableEnemy para manejarlo. Destruyendo directamente.", this);
                Destroy(gameObject);
            }
        }

        Debug.Log("HIT ENEMY GHOST");
        Debug.Log("Damage Taken: " + damageTaken);
    }
    public enum damageType
    {
        Resources,
        Durability,
        Headbutts
    }

}
