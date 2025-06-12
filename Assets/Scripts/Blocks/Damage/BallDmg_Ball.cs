using PrimeTween;
using UnityEngine;

public class BallDmg_Ball : MonoBehaviour
{

    public BallDmgBlock dmgBlock;
    public float damageCooldown = 2f;
    private float lastDamageTime = -Mathf.Infinity;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float time = Time.time;
            if (Time.time - lastDamageTime >= damageCooldown)
                if (HelmetManager.Instance.currentHelmet.helmetEffect != EffectTypeEnum.LavaResistance)
                {
                    dmgBlock.DoDamage();
                    dmgBlock.damageParticles.Play();
                }
        }
        if (HelmetManager.Instance.currentHelmet.helmetEffect != EffectTypeEnum.SlimeResistance)
        {
            dmgBlock.PushPlayerRandomly();
        }
            lastDamageTime = Time.time;
        }
    }

