using Unity.Cinemachine;
using UnityEngine;

public class DamageBlock : Block
{
    public damageType typeOfDamage;
    public int damage;
    public EffectTypeEnum helmetCounter;


    public Transform blockMeshParent;
    public GameObject blockMesh;
    public ParticleSystem damageParticles;
    public CinemachineImpulseSource impulseSource;

    [Header("SFX")]
    public AudioClip damageSound;
    private AudioSource audioSource;

    private void Start()
    {
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
            audioSource.PlayOneShot(damageSound, 0.7f);
            DoDamage();
        }
        
    }

    public override void Headbutt()
    {
        audioSource.PlayOneShot(damageSound, 0.7f);
        DoDamage();
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
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public enum damageType
    {
        Resources,
        Durability,
        Headbutts
    }
}
