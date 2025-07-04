using UnityEngine;

[System.Serializable]
public abstract class HelmetEffect
{
    public virtual bool hasSpecialAttack => false;

    public virtual void OnWear() { }
    public virtual void OnUnwear() { }
    public virtual void OnHeadbutt() { }
    public virtual void OnBounce() { }
    public virtual void OnSpecialAttack() { }
    public virtual void OnHitEnemy(GameObject enemy) { }
    
}
