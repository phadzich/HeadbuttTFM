using System;
using System.Collections.Generic;

[System.Serializable]
public class HelmetInstance
{
    public HelmetData baseHelmet;
    public string id;
    public int currentDurability;
    public int remainingHeadbutts;
    public int maxHeadbutts;
    public int durability;
    public bool isWornOut => currentDurability <= 0;
    public bool canBeUpgraded => level < 5;

    public int level = 1;

    public Action<HelmetInstance> HelmetInstanceChanged;// Evento que avisa que los stats fueron modificados

    public HelmetInstance(HelmetData helmetSO)
    {
        baseHelmet = helmetSO;
        id = Guid.NewGuid().ToString();
        currentDurability = helmetSO.durability;
        remainingHeadbutts = helmetSO.headbutts;
        maxHeadbutts = helmetSO.headbutts;
        durability = helmetSO.durability;
    }

    public void ResetStats()
    {
        currentDurability = durability;
        remainingHeadbutts = maxHeadbutts;
        HelmetInstanceChanged?.Invoke(this);
    }

    public void TakeDamage()
    {
        if (currentDurability > 0)
            currentDurability--;
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

    public void upgradeLevel()
    {
        level++;
    }
}
