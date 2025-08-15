using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthEBehaviour : MonoBehaviour, IEnemyBehaviour, IElementReactive
{
    public int maxHealth;
    public int currentHealth;
    public float damageMultiplier = 1;

    public bool isDead => currentHealth <= 0;

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    void RecieveDamage(int _amount)
    {
        Debug.Log($"DAMAGE: {_amount}");
        currentHealth -= _amount;
        if (isDead) Die();
    }

    void Heal(int _amount)
    {
        currentHealth = currentHealth + _amount >= maxHealth ? maxHealth : currentHealth + _amount;
    }

    void Die()
    {
        Debug.Log("DEAD");
        Destroy(this.gameObject);
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    public void StartBehaviour()
    {
        RestoreHealth();
    }

    public void StopBehaviour() { }

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
        if (targetElement == ElementType.Fire)
        {
            switch (sourceElement)
            {
                case ElementType.Water:
                    damageMultiplier = 2;
                    Debug.Log("Double Damage");
                    break;
                case ElementType.Fire:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
            }
        }

        if (targetElement == ElementType.Grass)
        {
            switch (sourceElement)
            {
                case ElementType.Fire:
                    damageMultiplier = 2;
                    Debug.Log("Double Damage");
                    break;
                case ElementType.Water:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
            }
        }
    }

    public void OnHit()
    {
        var _damage = ((int)HelmetManager.Instance.currentHelmet.baseHelmet.miningPower +1) * damageMultiplier;

        RecieveDamage((int)_damage);
        damageMultiplier = 1;
    }

    public bool IsAllowedForSource(InteractionSource source)
    {
        return AllowedSources.Count == 0 || AllowedSources.Contains(source);
    }
}

