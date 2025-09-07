using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/WaterShieldEffectData")]
public class ShieldEffectData : HelmetEffectData
{
    public float duration;

    public override HelmetEffect CreateEffect()
    {
        return new ElementalShield(this);
    }
}
