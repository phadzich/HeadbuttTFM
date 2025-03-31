using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public int sublevelId;
    public Vector2 sublevelPosition;


    public virtual void Bounce()
    {
    }

    public virtual void Headbutt()
    {

    }

    public virtual void Activate()
    {

    }
}
