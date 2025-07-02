using UnityEngine;

[System.Serializable]
public abstract class HelmetEffect : MonoBehaviour
{
    public virtual void OnHeadbutt() { }
    public virtual void OnBounce() { }
    public virtual void OnSpecialAttack() { }
    public virtual void OnHitEnemy(GameObject enemy) { }
}
