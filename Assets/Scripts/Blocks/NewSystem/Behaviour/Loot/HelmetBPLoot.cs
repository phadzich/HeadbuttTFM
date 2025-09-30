
using UnityEngine;

public class HelmetBPLoot : LootBase
{
    public HelmetData helmetBlueprint;
    public override Sprite GetIcon() => helmetBlueprint.icon;
    public override void Claim()
    {
        HelmetManager.Instance.Discover(helmetBlueprint);
    }
}
