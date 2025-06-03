using Unity.Cinemachine;
using UnityEngine;

public class NPCBlock : Block
{

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition= new Vector2(_xPos, _yPos);
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
