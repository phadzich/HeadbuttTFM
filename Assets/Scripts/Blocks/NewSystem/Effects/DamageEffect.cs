using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class DamageEffect : MonoBehaviour, IBlockEffect
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

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        DoDamage();

        if (breaksCombo)
        {
            MatchManager.Instance.EnemyBounced();
        }
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
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

    public void Activate()
    {
        throw new System.NotImplementedException();
    }

    public enum damageType
    {
        Resources,
        Durability,
        Headbutts
    }
}
