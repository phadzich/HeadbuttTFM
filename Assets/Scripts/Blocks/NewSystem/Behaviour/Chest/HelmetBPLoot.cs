using Mono.Cecil;
using UnityEngine;

public class HelmetBPLoot : LootBase
{
    public HelmetData helmetBlueprint;
    public override Sprite GetIcon() => helmetBlueprint.icon;
    public override void Claim() => Debug.Log($"CLAIMED {helmetBlueprint.helmetName}");
}
