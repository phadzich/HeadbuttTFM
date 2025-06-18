using Unity.Cinemachine;
using UnityEngine;
using System.Linq;

public class DamageBlock : Block
{
    public damageType typeOfDamage;
    public int damage;
    public EffectTypeEnum helmetCounter;
    public bool breaksCombo;

    public Transform blockMeshParent;
    public GameObject blockMesh;
    public ParticleSystem damageParticles;
    public CinemachineImpulseSource impulseSource;

    [Header("KNOCKBACK")]
    Block[] directions = new Block[4];
    public Vector3 currentPos;
    public Vector3 newDirection;

    [Header("SFX")]
    public AudioClip damageSound;
    public AudioSource audioSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
        isWalkable = true;
    }

    public override void Bounce()
    {
        if (HelmetManager.Instance.currentHelmet.helmetEffect != helmetCounter)
        {
            DoDamage();
        }
        if (breaksCombo)
        {
            MatchManager.Instance.EnemyBounced();
        }
    }

    public override void Headbutt()
    {
        DoDamage();
        if (breaksCombo)
        {
            MatchManager.Instance.EnemyBounced();
        }
    }

    public override void Activate()
    {

    }

    public void DoDamage()
    {
        //Debug.Log($"DAMAGED {typeOfDamage}:{damage}");
        if(typeOfDamage== damageType.Durability)
        {
            HelmetManager.Instance.currentHelmet.TakeDamage(damage);
        }
        ScreenShake();
        PlayDamageSound();
        damageParticles.Play();
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    private void PlayDamageSound()
    {
        audioSource.PlayOneShot(damageSound, 0.7f);
    }

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



    public enum damageType
    {
        Resources,
        Durability,
        Headbutts
    }
}
