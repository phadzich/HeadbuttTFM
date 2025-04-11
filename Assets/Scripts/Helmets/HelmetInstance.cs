using System;

[System.Serializable]
public class HelmetInstance
{
    public HelmetData baseHelmet;
    public string id;
    public int remainingBounces;
    public int remainingHeadbutts;
    public bool isWornOut => remainingBounces <= 0;

    public HelmetInstance(HelmetData helmetSO)
    {
        baseHelmet = helmetSO;
        id = Guid.NewGuid().ToString();
        remainingBounces = helmetSO.bounces;
        remainingHeadbutts = helmetSO.headbutts;
    }

    public void ResetStats()
    {
        remainingBounces = baseHelmet.bounces;
        remainingHeadbutts = baseHelmet.headbutts;
    }

    public void UseBounce()
    {
        if (remainingBounces > 0)
            remainingBounces--;
    }

    public void UseHeadbutt()
    {
        if (remainingHeadbutts > 0)
            remainingHeadbutts--;
    }

    public bool hasBouncesLeft()
    {
        return remainingBounces > 0;
    }

    public bool hasHeadbutts()
    {
        return remainingHeadbutts > 0;
    }
}
