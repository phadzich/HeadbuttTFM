using UnityEngine;

public class ResourceLoot : LootBase
{
    public ResourceData resource;
    public override Sprite GetIcon() => resource.icon;
    public override void Claim()
    {
        Debug.Log($"{amount} {resource}");
        ResourceManager.Instance.AddResource(resource,amount);
    }
}
