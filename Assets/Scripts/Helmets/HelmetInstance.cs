using System;

[System.Serializable]
public class HelmetInstance
{
    public HelmetData baseHelmet;
    public string id;
    public int remainingBounces;
    public int remainingHeadbutts;
    public int maxHeadbutts;
    public bool isWornOut => remainingBounces <= 0;

    private int level = 0;
    public int bounces;

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
        HelmetManager.Instance.onHelmetInstanceDataChanged?.Invoke(this);
    }

    public void UseBounce()
    {
        if (remainingBounces > 0)
            remainingBounces--;
        HelmetManager.Instance.onHelmetInstanceDataChanged?.Invoke(this);
    }

    public void UseHeadbutt()
    {
        if (remainingHeadbutts > 0)
            remainingHeadbutts--;
        HelmetManager.Instance.onHelmetInstanceDataChanged?.Invoke(this);
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
        // Por ahora reiniciare sus stats cuando lo mejoren
        ResetStats();
    }

    public void upgradeHeadbutt(int quantity)
    {
        maxHeadbutts += quantity;
        // Por ahora reiniciare sus stats cuando lo mejoren 
        ResetStats();
    }
}
