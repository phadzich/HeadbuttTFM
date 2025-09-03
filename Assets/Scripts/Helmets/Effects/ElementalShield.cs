using UnityEngine;

[System.Serializable]
public class ElementalShield : HelmetEffect
{
    private readonly ShieldEffectData data;
    private float duration;

    public ElementalShield(ShieldEffectData _data)
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
        if(PlayerManager.Instance.playerMovement.blockNSBelow.Element == HelmetManager.Instance.currentHelmet.Element)
        {
            PlayerManager.Instance.playerEffects.ShieldOn(duration);
        }
    }

}
