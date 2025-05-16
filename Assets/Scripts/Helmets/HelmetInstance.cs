using System;
using System.Collections.Generic;

[System.Serializable]
public class HelmetInstance
{
    public HelmetData baseHelmet;
    public string id;
    public int remainingBounces;
    public int remainingHeadbutts;
    public int maxHeadbutts;
    public int bounces;
    public bool isWornOut => remainingBounces <= 0;
    public bool canBeUpgraded => level < 5;

    public int level = 1;

    public Action<HelmetInstance> HelmetInstanceChanged;// Evento que avisa que los stats fueron modificados

    public HelmetInstance(HelmetData helmetSO)
    {
        baseHelmet = helmetSO;
        id = Guid.NewGuid().ToString();
        remainingBounces = helmetSO.bounces;
        remainingHeadbutts = helmetSO.headbutts;
        maxHeadbutts = helmetSO.headbutts;
        bounces = helmetSO.bounces;
    }

    public void ResetStats()
    {
        remainingBounces = bounces;
        remainingHeadbutts = maxHeadbutts;
        HelmetInstanceChanged?.Invoke(this);
    }

    public void UseBounce()
    {
        if (remainingBounces > 0)
            remainingBounces--;
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

    public bool hasBouncesLeft()
    {
        return remainingBounces > 0;
    }

    public bool hasHeadbutts()
    {
        return remainingHeadbutts > 0;
    }

    public void upgradeJump(int quantity)
    {
        bounces += quantity;
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
