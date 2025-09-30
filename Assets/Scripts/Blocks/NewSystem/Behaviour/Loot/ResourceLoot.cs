using UnityEngine;

public class ResourceLoot : LootBase
{
    public ResourceData resource;
    public override Sprite GetIcon() => resource.icon;
    public override void Claim()
    {
        CombatLogHUD.Instance.AddLog(resource.icon, $"<b>{amount}</b> <b>{resource.shortName}</b> found in CHEST!");
        ResourceManager.Instance.AddResource(resource,amount);
    }
}
