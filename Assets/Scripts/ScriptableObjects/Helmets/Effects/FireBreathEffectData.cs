using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/FireBreathEffectData")]
public class FireBreathEffectData : HelmetEffectData
{
    public float damageArea = 1f;
    public float damage = 1f;
    public int hbPointsUsed = 2;

    public override HelmetEffect CreateEffect()
    {
        return new FireBreath(this);
    }
}