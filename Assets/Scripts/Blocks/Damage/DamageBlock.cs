using Unity.Cinemachine;
using UnityEngine;

public class DamageBlock : Block
{
    public damageType typeOfDamage;
    public int damage;



    public Transform blockMeshParent;
    public GameObject blockMesh;
    public ParticleSystem damageParticles;
    public CinemachineImpulseSource impulseSource;



    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
        isWalkable = true;
    }

    public override void Bounce()
    {
        DoDamage();
    }

    public override void Headbutt()
    {

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
