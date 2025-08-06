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
        transform.Rotate(0, Random.Range(0f, 360f), 0, Space.World);
    }

    public override void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {

    }

    public override void Activate()
    {

    }

}
