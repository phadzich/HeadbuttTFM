using Unity.Cinemachine;
using UnityEngine;

public class FloorBlock : Block
{


    public Transform blockMeshParent;
    public GameObject blockMesh;


    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
    }

    public override void Bounce()
    {
        MatchManager.Instance.BouncedOnNeutralBlock();

    }

    public override void Headbutt()
    {


    }

    public override void Activate()
    {

    }

}
