using UnityEngine;

[System.Serializable]
public abstract class LootBase : ILoot
{
    [SerializeField] private int chestID;

    public abstract Sprite GetIcon();
    public int targetId => chestID;

    public abstract void Claim();
    public int amount = 1;
}
