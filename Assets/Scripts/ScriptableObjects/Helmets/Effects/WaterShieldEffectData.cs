using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/WaterShieldEffectData")]
public class WaterShieldEffectData : HelmetEffectData
{
    public override HelmetEffect CreateEffect()
    {
        return new WaterShield(this);
    }
}
