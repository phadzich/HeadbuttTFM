using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/WaterShieldEffectData")]
public class WaterShieldEffectData : HelmetEffectData
{
    public float duration;

    public override HelmetEffect CreateEffect()
    {
        return new WaterShield(this);
    }
}
