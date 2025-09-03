using UnityEngine;

[System.Serializable]
public class WaterShield : HelmetEffect
{
    private readonly WaterShieldEffectData data;
    private float duration;

    public WaterShield(WaterShieldEffectData _data)
    {
        data = _data;
        duration = _data.duration;
    }

    public override void OnUpgradeEffect(float stat)
    {
        duration = stat;
    }

    public override void OnHeadbutt()
    {
        if(PlayerManager.Instance.playerMovement.blockNSBelow.blockName == "WATER")
        {
            PlayerManager.Instance.playerEffects.ShieldOn(duration);
        }
    }

}
