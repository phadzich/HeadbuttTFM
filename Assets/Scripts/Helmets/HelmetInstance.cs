using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HelmetInstance
{
    //Helmet info
    public string id;
    public HelmetInfo currentInfo = new HelmetInfo();
    public HelmetData baseHelmet;
    public ElementData helmetElement;
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

    public Action<HelmetInstance> HelmetInstanceChanged;// Evento que avisa que los stats fueron modificados
    public Action OnDamaged;

    public HelmetInstance(HelmetData _helmetSO)
    {
        id = Guid.NewGuid().ToString();
        currentInfo = _helmetSO.helmetInfo.Copy();
        baseHelmet = _helmetSO;

        //Stats
        currentDurability = _helmetSO.durability;
        currentLevel = 0;
        durability = _helmetSO.durability;

        helmetElement = _helmetSO.element;
        currentHBHarvest = 0.3f;

    }

    public void ResetStats()
    {
        currentDurability = durability;
        HelmetInstanceChanged?.Invoke(this);
    }

    public void TakeDamage(int _amount, bool _isEnemy = false)
    {
        if(PlayerManager.Instance.onWaterShield && _isEnemy)
        {
            PlayerManager.Instance.DeactivateShield();
            return;
        }
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
        durability += _quantity;
        currentDurability = durability;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeCurrentEvolution(int _evolution)
    {
        currentLevel = _evolution;
    }


    public void UpdateInfo(HelmetInfo _newInfo)
    {
        currentInfo = _newInfo.Copy();
    }

    public void AddEffect(HelmetEffect _effect)
    {
        activeEffects.Add(_effect);
    }

    public void UpdateHelmetElement(ElementData _element)
    {
        helmetElement = _element;
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

    // Llamar cuando se presiona la tecla de special attack
    public void OnSpecialAttack()
    {
        foreach (HelmetEffect _effect in activeEffects)
        {
                _effect.OnSpecialAttack();
        }
    }

    public void Craft()
    {
        isCrafted = true;
        currentLevel++;
    }

    public void Discover()
    {
        isDiscovered = true;
    }

    /* Funciones para evolucionar el casco */

    public UpgradeRequirement GetUpgradeRequirement(int _toEvolution)
    {
        if(_toEvolution == 2)
        {
            return baseHelmet.levelUpRequirements[0];
        }

        return baseHelmet.levelUpRequirements[1];
    }

    // Llamar cuando se quiera evolucionar el casco, la funcion actualiza los stats
    public void Evolve(UpgradeRequirement req)
    {
        UpgradeCurrentEvolution(req.toEvolution);
        UpdateInfo(req.newInfo);

        UpgradeDurability(req.durabilityAdd);

        // Activamos el efecto del casco
        if (req.activateEffect)
        {
            foreach(var _effect in baseHelmet.effects)
            {
                AddEffect(_effect.CreateEffect());
            }
        }

        HelmetInstanceChanged?.Invoke(this);
    }

    public bool CanEvolve()
    {
        var playerResources = ResourceManager.Instance.ownedResources;

        UpgradeRequirement req = GetUpgradeRequirement(nextLevel);

        foreach (var requirement in req.requirements)
        {
            if (!ResourceManager.Instance.CanSpendResource(requirement.resource, requirement.quantity))
            {
                return false;
            }
        }
        return true;
    }

}
