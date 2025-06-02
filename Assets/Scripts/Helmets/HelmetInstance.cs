using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelmetInstance
{
    // Helmet info
    public HelmetData baseHelmet;
    public string id;
    public GameObject currentMesh => baseHelmet.meshesByLevel[helmetXP.currentLevel-1];
    public HelmetEffectType helmetEffect = HelmetEffectType.None;
    public int effectPower = 0;

    //Helmet Stats
    public int currentDurability;
    public int remainingHeadbutts;
    public int maxHeadbutts;
    public int durability;
    public HelmetXP helmetXP;

    public bool isWornOut => currentDurability <= 0;
    public bool canBeUpgraded => helmetXP.currentLevel < 3;

    public Action<HelmetInstance> HelmetInstanceChanged;// Evento que avisa que los stats fueron modificados

    public HelmetInstance(HelmetData helmetSO)
    {
        baseHelmet = helmetSO;
        id = Guid.NewGuid().ToString();
        currentDurability = helmetSO.durability;
        remainingHeadbutts = helmetSO.headbutts;
        maxHeadbutts = helmetSO.headbutts;
        durability = helmetSO.durability;
        helmetXP = new HelmetXP(helmetSO.baseXP, helmetSO.xpMultiplier,this);
        helmetXP.SubleveledUp += UpgradeStatsBySublevel;
        helmetXP.LeveledUp += UpgradeStatsByLevel;

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
        HelmetInstanceChanged?.Invoke(this);
    }

    public void UseHeadbutt()
    {
        if (remainingHeadbutts > 0)
            remainingHeadbutts--;
        //HelmetManager.Instance.onHelmetInstanceDataChanged?.Invoke(this);
        HelmetInstanceChanged?.Invoke(this);
    }

    public bool hasHeadbutts()
    {
        return remainingHeadbutts > 0;
    }

    public void IncreaseDurability(int quantity)
    {
        durability += quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void upgradeHeadbutt(int quantity)
    {
        maxHeadbutts += quantity;
        // reiniciar sus stats cuando lo mejoren
    }

    public void UpgradeStatsBySublevel(int currentSublevel)
    {
        switch (helmetXP.currentLevel) {
            case 1:
                if (currentSublevel == 2)
                    IncreaseDurability(1);
                else if (currentSublevel == 3)
                    upgradeHeadbutt(1);
                else if (currentSublevel == 5)
                {
                    IncreaseDurability(1);
                    upgradeHeadbutt(1);
                }
                break;

            case 2:
                if (currentSublevel == 1)
                    IncreaseDurability(1);
                else if (currentSublevel == 3)
                    effectPower += 1;
                else if (currentSublevel == 4)
                    upgradeHeadbutt(1);
                else if (currentSublevel == 5)
                {
                    IncreaseDurability(1);
                    effectPower += 1;
                }
                break;

            case 3:
                if (currentSublevel == 1)
                    IncreaseDurability(1);
                else if (currentSublevel == 3)
                    effectPower += 1;
                else if (currentSublevel == 4)
                    upgradeHeadbutt(1);
                else if (currentSublevel == 5)
                {
                    IncreaseDurability(1);
                    effectPower += 1;
                }
                break;
        }
    }

    public void UpgradeStatsByLevel(int currentLevel)
    {
        if (currentLevel == 2)
        {
            helmetEffect = baseHelmet.helmetEffect;
            effectPower = 1; // Se desbloquea con poder inicial
        }
        else if (currentLevel == 3)
        {
            // Aquí podemos aplicar una mejora final especial
            effectPower += 1;
        }

        HelmetInstanceChanged?.Invoke(this);
    }

    public List<ResourceRequirement> GetPriceForNextLevel()
    {
        int nextLevel = helmetXP.currentLevel + 1;
        var requirement = baseHelmet.upgradePrices.Find(r => r.toLevel == nextLevel);
        return requirement?.requirements ?? new List<ResourceRequirement>();
    }

}
