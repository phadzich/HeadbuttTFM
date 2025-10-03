using NUnit.Framework.Interfaces;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemLore;
    [TextArea] public string itemDescription;

    public Sprite effectIcon;
    public Sprite illustration;
    public int value;

    public PotionTypes type;

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
