using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthEBehaviour : MonoBehaviour, IEnemyBehaviour, IElementReactive
{

    public HealthBarUI healthBarUI;
    public int maxHealth;
    public int currentHealth;
    public float damageMultiplier = 1;

    public bool isDead => currentHealth <= 0;

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    void RecieveDamage(int _amount)
    {
        //Debug.Log($"DAMAGE: {_amount}");
        currentHealth -= _amount;
        if (isDead) Die();
        UpdateBarUI();
    }

    void Heal(int _amount)
    {
        currentHealth = currentHealth + _amount >= maxHealth ? maxHealth : currentHealth + _amount;
        UpdateBarUI();
    }

    void Die()
    {
        //Debug.Log("DEAD");
        DispatchDeathEvent();
        Destroy(this.gameObject);

    }
    private void DispatchDeathEvent()
    {
        var _enemyDeathEvent = new EnemyDeathsEvent {};
        LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_enemyDeathEvent);
    }
    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        UpdateBarUI();
    }

    public void StartBehaviour()
    {
        RestoreHealth();
    }

    private void UpdateBarUI()
    {
        healthBarUI.UpdateBar(currentHealth, maxHealth);
    }

    public void StopBehaviour() { }

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
        Debug.Log("checking interactions");
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

