using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelmetInstance
{
    //Helmet info
    public string id;
    public HelmetInfo currentInfo = new HelmetInfo();
    public HelmetData baseHelmet;
    public ElementEnum helmetElement;

    //Helmet Stats
    public int durability;
    public float headBForce;
    public int currentEvolution;
    public int nextEvolution => currentEvolution + 1;

    // Efectos and overcharged
    public EffectTypeEnum helmetEffect;
    public OverchargeEffectEnum overchargeEffect;

    public EffectTypeEnum activeEffect;
    public OverchargeEffectEnum activeOverchargeEffect;

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
        currentEvolution = _helmetSO.evolution;
        durability = _helmetSO.durability;
        headBForce = _helmetSO.headBForce;

        //Effects
        helmetEffect = _helmetSO.effect;
        overchargeEffect = _helmetSO.overchargeEffect;

        // Se inician inactivos
        activeEffect = EffectTypeEnum.None;
        activeOverchargeEffect = OverchargeEffectEnum.None;

        helmetElement = ElementEnum.None;
        currentHBHarvest = 0.3f;

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
        durability += _quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeHeadBForce(float _quantity)
    {
        headBForce += _quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeCurrentEvolution(int _evolution)
    {
        currentEvolution = _evolution;
    }


    public void UpdateInfo(HelmetInfo _newInfo)
    {
        currentInfo = _newInfo.Copy();
    }

    public void UpdateHelmetEffect(EffectTypeEnum _effect)
    {
        helmetEffect = _effect;
    }

    public void UpdateHelmetElement(ElementEnum _element)
    {
        helmetElement = _element;
    }

    /* Funciones para evolucionar el casco */

    public UpgradeRequirement GetUpgradeRequirement(int _toEvolution)
    {
        if(_toEvolution == 2)
        {
            return baseHelmet.upgradeRequirements[0];
        }

        return baseHelmet.upgradeRequirements[1];
    }

    // Llamar cuando se quiera evolucionar el casco, la funcion actualiza los stats
    public void Evolve(UpgradeRequirement req)
    {
        UpgradeCurrentEvolution(req.toEvolution);
        UpdateInfo(req.newInfo);

        UpgradeDurability(req.durabilityAdd);
        UpgradeHeadBForce(req.HBForceAdd);

        // Activamos el efecto del casco
        if (req.activateEffect)
        {
            activeEffect = helmetEffect;
        }

        // Activamos el efecto overcharge del cascp
        if (req.isOvercharged)
        {
            activeOverchargeEffect = overchargeEffect;
        }
    }

    public bool CanEvolve()
    {
        var playerResources = ResourceManager.Instance.ownedResources;

        UpgradeRequirement req = GetUpgradeRequirement(nextEvolution);

        foreach (var requirement in req.requirements)
        {
            if (!ResourceManager.Instance.CanSpendResource(requirement.resource, requirement.quantity))
            {
                return false;
            }
        }
        return true;
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

}
