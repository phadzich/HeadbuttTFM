using NUnit.Framework.Interfaces;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite illustration;
    public virtual void Buy()
    {
    }
    public virtual void Sell()
    {
    }
    public virtual void Use()
    {
    }
}
