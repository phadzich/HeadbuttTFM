using UnityEngine;

[System.Serializable]
public abstract class Enemy: MonoBehaviour
{
    public int life;

    public virtual void OnHit(int damage){}
}
