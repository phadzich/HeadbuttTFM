[System.Serializable]
public class HelmetInstance
{
    public HelmetData baseHelmet;
    public int remainingBounces;
    public int remainingHeadbutts;

    public HelmetInstance(HelmetData helmetSO)
    {
        baseHelmet = helmetSO;
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
}
