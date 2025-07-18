using UnityEngine;

[System.Serializable]
public class WaterShield : HelmetEffect
{
    private readonly WaterShieldEffectData data;
    public WaterShield(WaterShieldEffectData _data)
    {
        data = _data;
    }

    public override void OnHeadbutt()
    {
        Debug.Log(PlayerManager.Instance.playerMovement.blockBelow.ToString());
        if(PlayerManager.Instance.playerMovement.blockBelow.ToString() == "DBG_WATER(Clone) (DamageBlock)")
        {
            PlayerManager.Instance.ActivateShield();
        }
    }

}
