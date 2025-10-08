using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthEBehaviour : MonoBehaviour, IEnemyBehaviour, IElementReactive
{

    public HealthBarUI healthBarUI;
    public int maxHealth;
    public int currentHealth;
    public float damageMultiplier = 1;
    private ElementType lastElement;

    public bool isDead => currentHealth <= 0;
    public event Action<HealthEBehaviour> OnDeath;

    private EnemySFX sfx => GetComponent<EnemySFX>();

    [SerializeField] public List<InteractionSource> AllowedSources = new List<InteractionSource>();

    void RecieveDamage(int _amount, ElementType _element)
    {
        //CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.enemyReq, $"Enemy attacked with <b>{_amount} {_element}</b>!");
        StartCoroutine(PlayJomaSound(0.2f));
        if (sfx != null) sfx.PlayDamage();
        //Debug.Log($"DAMAGE: {_amount}");
        currentHealth -= _amount;
        if (isDead) Die();
        UpdateBarUI();
        healthBarUI.PopDamage(_amount, _element);


    }

    private IEnumerator PlayJomaSound(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SoundManager.PlayJomaSound(JomaType.ATTACK);

    }

    void Heal(int _amount)
    {
        currentHealth = currentHealth + _amount >= maxHealth ? maxHealth : currentHealth + _amount;
        UpdateBarUI();
    }

    void Die()
    {
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.enemyReq, $"Enemy <b>KILLED</b>!");
        OnDeath?.Invoke(this);
        if (sfx != null) sfx.PlayDeath();
        DispatchDeathEvent();
        Destroy(this.gameObject);
        ResourceManager.Instance.coinTrader.AddCoins(maxHealth);

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
        lastElement = ElementType.Neutral;
    }

    private void UpdateBarUI()
    {
        healthBarUI.UpdateBar(currentHealth, maxHealth);
    }

    public void StopBehaviour() { }

    public void OnElementInteraction(ElementType sourceElement, ElementType targetElement)
    {
        lastElement = sourceElement;
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
                case ElementType.Grass:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
                case ElementType.Electric:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
                case ElementType.Neutral:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
                case ElementType.None:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
            }
        }

        if (targetElement == ElementType.Water)
        {
            switch (sourceElement)
            {
                case ElementType.Water:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
                case ElementType.Fire:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
                case ElementType.Grass:
                    damageMultiplier = 2;
                    Debug.Log("Double Damage");
                    break;
                case ElementType.Electric:
                    damageMultiplier = 2;
                    Debug.Log("Double Damage");
                    break;
                case ElementType.Neutral:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
                case ElementType.None:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
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
                case ElementType.Grass:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
                case ElementType.Electric:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
                case ElementType.Neutral:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
                case ElementType.None:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
            }
        }

        if (targetElement == ElementType.Electric)
        {
            switch (sourceElement)
            {
                case ElementType.Fire:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
                case ElementType.Water:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
                case ElementType.Grass:
                    damageMultiplier = 2;
                    Debug.Log("Double Damage");
                    break;
                case ElementType.Electric:
                    damageMultiplier = 0;
                    Debug.Log("NO Damage");
                    break;
                case ElementType.Neutral:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
                case ElementType.None:
                    damageMultiplier = 1;
                    Debug.Log("Normal Damage");
                    break;
            }
        }

        if (targetElement == ElementType.Neutral)
        {
            damageMultiplier = 1;
            Debug.Log("Normal Damage");
        }


    }

    public void OnHit()
    {
        var _damage = ((int)HelmetManager.Instance.currentHelmet.baseHelmet.miningPower +1) * damageMultiplier;

        RecieveDamage((int)_damage, lastElement);
        damageMultiplier = 1;
    }

    public bool IsAllowedForSource(InteractionSource source)
    {
        return AllowedSources.Count == 0 || AllowedSources.Contains(source);
    }
}

