using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelmetInstance
{
    //Helmet info
    public string id;
    public HelmetInfo currentInfo = new HelmetInfo();

    //Helmet Stats
    public int maxHeadbutts;
    public int durability;
    public float bounceHeight;
    public int headBForce;
    public float headBCooldown;
    public int knockbackChance;
    public HelmetXP helmetXP;
    public EffectTypeEnum helmetEffect;
    public ElementEnum helmetElement;

    //Current stats
    public int currentDurability;
    public int remainingHeadbutts;

    public bool HasHeadbutts => remainingHeadbutts > 0;
    public bool IsWornOut => currentDurability <= 0;

    public Action<HelmetInstance> HelmetInstanceChanged;// Evento que avisa que los stats fueron modificados

    public HelmetInstance(HelmetData _helmetSO)
    {
        id = Guid.NewGuid().ToString();
        currentInfo = _helmetSO.helmetInfo.Copy();

        //Stats
        currentDurability = _helmetSO.durability;
        remainingHeadbutts = _helmetSO.headbutts;
        maxHeadbutts = _helmetSO.headbutts;
        durability = _helmetSO.durability;
        bounceHeight = _helmetSO.bounceHeight;
        headBForce = _helmetSO.headBForce;
        headBCooldown = _helmetSO.headBCooldown;
        knockbackChance = _helmetSO.knockbackChance;
        helmetXP = new HelmetXP(_helmetSO.baseXP, _helmetSO.xpMultiplier,this);
        helmetEffect = EffectTypeEnum.None;
        helmetElement = ElementEnum.None;

    }

    public void ResetStats()
    {
        currentDurability = durability;
        remainingHeadbutts = maxHeadbutts;
        HelmetInstanceChanged?.Invoke(this);
    }

    public void TakeDamage(int _amount)
    {
        if (currentDurability > 0)
            currentDurability-=_amount;
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

        HelmetInstanceChanged?.Invoke(this);
    }

    public void UseHeadbutt()
    {
        if (remainingHeadbutts > 0)
            remainingHeadbutts--;
        //HelmetManager.Instance.onHelmetInstanceDataChanged?.Invoke(this);
        HelmetInstanceChanged?.Invoke(this);
    }

    /* Funciones para hacer upgrade a los stats y modificar la info del casco*/

    public void UpgradeDurability(int _quantity)
    {
        durability += _quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeHeadbutt(int _quantity)
    {
        maxHeadbutts += _quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeBounceHeight(float _quantity)
    {
        bounceHeight += _quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeHeadBForce(int _quantity)
    {
        headBForce += _quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeHeadBCooldown(float _quantity)
    {
        headBCooldown += _quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeKnockbackChance(int _quantity)
    {
        knockbackChance += _quantity;
        // reiniciar sus stats cuando lo mejoren
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

    public void Evolve(HelmetBlueprint _blueprint)
    {
        helmetXP.Evolve(_blueprint.baseXP, _blueprint.xpMultiplier);
        UpdateHelmetEffect(_blueprint.effect);
        UpdateHelmetElement(_blueprint.element);
        UpdateInfo(_blueprint.helmetInfo);
    }

}
