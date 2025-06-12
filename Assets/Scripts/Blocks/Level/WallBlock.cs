using Unity.Cinemachine;
using UnityEngine;

public class WallBlock : Block
{


    public Transform blockMeshParent;
    public GameObject blockMesh;


    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelPosition = new Vector2(_xPos, _yPos);
        sublevelId = _subId;
        isWalkable = false;
    }

    public override void Bounce()
    {
    }

    public override void Headbutt()
    {

    }

    public override void Activate()
    {

    }

}
