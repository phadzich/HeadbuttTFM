using Unity.Cinemachine;
using UnityEngine;

public class NPCBlock : Block
{

   
    public Transform blockMeshParent;
    public GameObject blockMesh;

    CinemachineImpulseSource impulseSource;

    private void Start()
    {

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
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

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }



}
