
using UnityEngine;

public class ItemLoot : LootBase
{
    public Item item;
    public override Sprite GetIcon() => item.illustration;
    public override void Claim()
    {
        InventoryManager.Instance.itemsInventory.TryAddOwnedItems(item, amount);
    }
}