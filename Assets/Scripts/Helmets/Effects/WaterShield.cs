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
        Debug.Log(PlayerManager.Instance.playerMovement.blockBelow.ToString());
        if(PlayerManager.Instance.playerMovement.blockBelow.ToString() == "DBG_WATER(Clone) (DamageBlock)")
        {
            PlayerManager.Instance.ActivateShield(duration);
        }
    }

}
