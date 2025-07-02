using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/VolcanicRecharge")]
public class RechargeEffectData : HelmetEffectData
{
    public float damage = 10f;
    public float harvest = 1f;
    public int maxPower = 10;

    public override HelmetEffect CreateEffect()
    {
        return new VolcanicRecharge(this);
    }
}