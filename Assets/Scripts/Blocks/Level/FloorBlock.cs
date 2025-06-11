using Unity.Cinemachine;
using UnityEngine;

public class FloorBlock : Block
{


    public Transform blockMeshParent;
    public GameObject blockMesh;


    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelPosition = new Vector2(_xPos, _yPos);
        sublevelId = _subId;
        isWalkable = true;
    }

    public override void Bounce()
    {
        MatchManager.Instance.FloorBounced();

    }

    public override void Headbutt()
    {


    }

    public override void Activate()
    {

    }

}
