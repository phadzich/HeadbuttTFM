using UnityEngine;

[System.Serializable]
public abstract class Enemy: MonoBehaviour
{
    public virtual void OnHit(){}
}
