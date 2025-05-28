using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public int sublevelId;
    public Vector2 sublevelPosition;
    public bool isWalkable = false;

    // Vecinos
    public Block up;
    public Block down;
    public Block left;
    public Block right;

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
