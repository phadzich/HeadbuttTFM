using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelmetInstance
{
    //Helmet info
    public string id;
    public HelmetInfo currentInfo;

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
        currentInfo.mesh = _helmetSO.helmetInfo.mesh;
        currentInfo.icon = _helmetSO.helmetInfo.icon;
        currentInfo.color = _helmetSO.helmetInfo.color;
        currentInfo.description = _helmetSO.helmetInfo.description;
        currentInfo.name = _helmetSO.helmetInfo.name;

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

    public void UpgradeDurability(int quantity)
    {
        durability += quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeHeadbutt(int quantity)
    {
        maxHeadbutts += quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeBounceHeight(float quantity)
    {
        bounceHeight += quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeHeadBForce(int quantity)
    {
        headBForce += quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeHeadBCooldown(float quantity)
    {
        headBCooldown += quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeKnockbackChance(int quantity)
    {
        knockbackChance += quantity;
        // reiniciar sus stats cuando lo mejoren
    }



}
