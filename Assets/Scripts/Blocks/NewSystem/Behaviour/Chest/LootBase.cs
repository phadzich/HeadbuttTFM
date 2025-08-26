using UnityEngine;

[System.Serializable]
public abstract class LootBase : ILoot
{
    [SerializeField] private int customID;
    public int targetId => customID;
    public abstract void Claim();
}
