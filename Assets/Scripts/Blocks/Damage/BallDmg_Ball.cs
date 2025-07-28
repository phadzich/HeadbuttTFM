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
            if (Time.time - lastDamageTime >= damageCooldown) dmgBlock.DoDamage();
            SoundManager.PlaySound(SoundType.FIREDAMAGE, 0.7f);
        }
            lastDamageTime = Time.time;
        }
    }

