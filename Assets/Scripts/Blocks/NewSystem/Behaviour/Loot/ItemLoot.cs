using Mono.Cecil;
using UnityEngine;

public class ItemLoot : LootBase
{
    public Item item;
    public override Sprite GetIcon() => item.illustration;
    public override void Claim()
    {
        Debug.Log($"CLAIMED {item.itemName}");
        InventoryManager.Instance.itemsInventory.TryAddOwnedItems(item, amount);
    }
}