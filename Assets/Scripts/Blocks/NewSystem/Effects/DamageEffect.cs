using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class DamageEffect : MonoBehaviour, IBlockEffect
{
    public damageType typeOfDamage;
    public int damage;
    public bool breaksCombo;
    public bool stopDamage;

    public ParticleSystem damageParticles;
    public CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        if (stopDamage) return;

        DoDamage();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        if (stopDamage) return;

        DoDamage();
    }

    public void Activate()
    {
        stopDamage = !stopDamage;
    }

    public void DoDamage()
    {
        if (breaksCombo)
        {
            MatchManager.Instance.EnemyBounced();
        }

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
        damageParticles.Play();
    }

    private void DurabilityDamage()
    {
        PlayerManager.Instance.playerEffects.TakeDamage(damage);
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

    public enum damageType
    {
        Resources,
        Durability,
        Headbutts
    }
}
