using UnityEngine;

[System.Serializable]
public class VolcanicRecharge : HelmetEffect
{
    private readonly RechargeEffectData data;

    public VolcanicRecharge(RechargeEffectData _data)
    {
        data = _data;
    }

    public override void OnBounce()
    {
        // recarga el poder al saltar en bloques de lava
        Debug.Log($"¡Explota! Daño: {data.damage}, Harvest: {data.harvest}");
    }

    public override void OnHitEnemy(GameObject enemy)
    {
        // usa el poder
    }
}
