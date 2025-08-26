using UnityEngine;

public class ItemReward : LootBase
{
    public Item item;
    public int amount;
    public override void Claim() => Debug.Log($"CLAIMED {item.itemName}");
}