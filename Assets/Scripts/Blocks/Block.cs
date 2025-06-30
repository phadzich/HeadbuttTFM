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

    public virtual void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public virtual void OnHeadbutted(HelmetInstance _helmetInstance)
    {

    }

    public virtual void Activate()
    {

    }
}
