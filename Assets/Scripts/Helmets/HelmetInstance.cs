using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HelmetInstance: IElemental
{
    //Helmet info
    public string id;
    public HelmetData baseHelmet;
    public bool isCrafted = false;
    public bool isDiscovered = false;

    //Helmet Stats
    public int durability;
    public int currentLevel;
    public int nextLevel => currentLevel + 1;

    // Efectos and overcharged
    [SerializeField]
    public List<HelmetEffect> activeEffects = new List<HelmetEffect>();

    //Current stats
    public int currentDurability;
    public float currentHBHarvest;

    public bool IsWornOut => currentDurability <= 0;

    public ElementType Element => baseHelmet.element;

    public Action<HelmetInstance> HelmetInstanceChanged;// Evento que avisa que los stats fueron modificados
    public Action OnDamaged;

    public HelmetInstance(HelmetData _helmetSO)
    {
        id = Guid.NewGuid().ToString();
        baseHelmet = _helmetSO;

        //Stats
        currentDurability = 0;
        currentLevel = 0;
        durability = 0;

        currentHBHarvest = 0.3f;

        //Debug.Log(_helmetSO.helmetName);

        ActivateEffects(_helmetSO.effects);

    }

    public void ActivateEffects(List<HelmetEffectData> _effects)
    {
        //Debug.Log(_effects);

        foreach (var _effect in _effects)
        {
            if (_effect == null)
            {
                //Debug.Log("HAY UNA LISTA CREADA PERO EL EFECTO ES NULL"); 
                continue;
            }
            AddEffect(_effect.CreateEffect());
        }
    }

    public void ResetStats()
    {
        currentDurability = durability;
        HelmetInstanceChanged?.Invoke(this);
    }

    public void TakeDamage(int _amount)
    {
        //Debug.Log($"Helmet Took {_amount}");
        if (currentDurability > 0)
            currentDurability-=_amount;
        //Debug.Log($"Current Durability {currentDurability}");
        //HelmetManager.Instance.onHelmetInstanceDataChanged?.Invoke(this);
        PlayerManager.Instance.damageTakenIndicator.AnimateDamage(_amount);
            if (IsWornOut)
            {
            PlayerManager.Instance.RemovePlayerLives(1);
                if (HelmetManager.Instance.HasHelmetsLeft)
                {
                    HelmetManager.Instance.WearNextAvailableHelmet();
                }
            }

        OnDamaged?.Invoke();
        HelmetInstanceChanged?.Invoke(this);
    }

    /* Funciones para hacer upgrade a los stats y modificar la info del casco*/

    public void UpgradeDurability(int _quantity)
    {
        durability = _quantity;
        currentDurability = durability;
        // reiniciar sus stats cuando lo mejoren
    }

    public void LevelUp(int _level)
    {
        currentLevel = _level;
    }

    public void AddEffect(HelmetEffect _effect)
    {
        activeEffects.Add(_effect);
    }

    public void HealDurability(int _amount)
    {
        if (_amount > durability - currentDurability)
        {
            currentDurability = durability;
        }
        else
        {
            currentDurability += _amount;
        }
        HelmetInstanceChanged?.Invoke(this);
    }

    // Llamar cuando se hace un HB
    public void OnHeadbutt()
    {
        foreach (var effect in activeEffects)
        {
            effect.OnHeadbutt();
        }
    }

    // Llamar en cada salto
    public void OnBounce()
    {
        foreach (var effect in activeEffects)
        {
            effect.OnBounce();
        }
    }
    public void OnWear()
    {
        foreach (HelmetEffect _effect in activeEffects)
        {
            _effect.OnWear();
        }
    }

    // Llamar cuando se presiona la tecla de special attack
    public void OnUpgradeEffect(float _stat)
    {
        foreach (HelmetEffect _effect in activeEffects)
        {
            _effect.OnUpgradeEffect(_stat);
        }
    }

    public void Craft()
    {
        isCrafted = true;
        LevelUpHelmet(GetUpgradeRequirement());
        HelmetInstanceChanged?.Invoke(this);
    }

    public void Discover()
    {
        isDiscovered = true;
    }

    /* Funciones para evolucionar el casco */

    public UpgradeRequirement GetUpgradeRequirement()
    {
        return baseHelmet.levelUpRequirements[currentLevel];
    }

    // Llamar cuando se quiera evolucionar el casco, la funcion actualiza los stats
    public void LevelUpHelmet(UpgradeRequirement req)
    {
        LevelUp(nextLevel);

        UpgradeDurability(req.durability);
        OnUpgradeEffect(req.powerStat);

        HelmetInstanceChanged?.Invoke(this);

    }

}
