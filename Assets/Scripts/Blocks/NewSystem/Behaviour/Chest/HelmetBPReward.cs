using UnityEngine;

public class HelmetBPReward : LootBase
{
    public HelmetData helmetBlueprint;
    public override void Claim() => Debug.Log($"CLAIMED {helmetBlueprint.helmetName}");
}
