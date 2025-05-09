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

    public Action<HelmetInstance> HelmetInstanceChanged;

    public HelmetInstance(HelmetData helmetSO)
    {
        baseHelmet = helmetSO;
        id = Guid.NewGuid().ToString();
        remainingBounces = helmetSO.bounces;
        remainingHeadbutts = helmetSO.headbutts;
        maxHeadbutts = helmetSO.headbutts;
    }

    public void ResetStats()
    {
        remainingBounces = baseHelmet.bounces;
        remainingHeadbutts = baseHelmet.headbutts;
        //HelmetManager.Instance.onHelmetInstanceDataChanged?.Invoke(this);
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
}
