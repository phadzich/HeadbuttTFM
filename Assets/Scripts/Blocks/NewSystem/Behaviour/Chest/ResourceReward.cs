using UnityEngine;

public class ResourceReward : LootBase
{
    public ResourceData resource;
    public int amount;
    public override void Claim() => Debug.Log($"{amount} {resource}");
}
