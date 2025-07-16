using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public string itemName;
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
