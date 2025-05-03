using Unity.Cinemachine;
using UnityEngine;

public class DamageBlock : Block
{
    public damageType typeOfDamage;
    public int damage;



    public Transform blockMeshParent;
    public GameObject blockMesh;
    public ParticleSystem damageParticles;
    CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
    }

    public override void Bounce()
    {
        Debug.Log("DMGBLCK BOUNCED");
        DoDamage();
        ScreenShake();
        damageParticles.Play();
    }

    public override void Headbutt()
    {

    }

    public override void Activate()
    {

    }

    public void DoDamage()
    {
        Debug.Log($"DAMAGED {typeOfDamage}:{damage}");
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public enum damageType
    {
        Resources,
        Bounces,
        Headbutts
    }
}
