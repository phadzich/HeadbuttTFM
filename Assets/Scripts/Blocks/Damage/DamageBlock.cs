using Unity.Cinemachine;
using UnityEngine;
using System.Linq;

public class DamageBlock : Block
{
    public damageType typeOfDamage;
    public int damage;
    public bool breaksCombo;

    public ParticleSystem damageParticles;
    public CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
        isWalkable = true;
    }

    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        DoDamage();

        if (breaksCombo)
        {
            MatchManager.Instance.EnemyBounced();
        }
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        DoDamage();
        if (breaksCombo)
        {
            MatchManager.Instance.EnemyBounced();
        }
    }

    public void DoDamage()
    {
        //Debug.Log($"DAMAGED {typeOfDamage}:{damage}");
        switch (typeOfDamage)
        {
            case damageType.Durability:
                DurabilityDamage();
                break;
            case damageType.Resources:
                ResourcesDamage();
                break;
            case damageType.Headbutts:
                HeadbuttDamage();
                break;
        }

        ScreenShake();
        PlayDamageSound();
        damageParticles.Play();
    }

    private void DurabilityDamage()
    {
        HelmetManager.Instance.currentHelmet.TakeDamage(damage, true);
    }

    private void ResourcesDamage()
    {
        // TODO
    }

    private void HeadbuttDamage()
    {
        // TODO
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public void PlayDamageSound()
    {
        SoundManager.PlaySound(SoundType.FIREDAMAGE, 0.7f);
    }

    public void BallDmgSound()
    {
        SoundManager.PlaySound(SoundType.PUSHDAMAGE, 0.7f);
    }

    public enum damageType
    {
        Resources,
        Durability,
        Headbutts
    }
}
