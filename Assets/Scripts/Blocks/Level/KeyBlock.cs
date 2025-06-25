using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class KeyBlock : Block
{
    public int keyID;
    public GameObject keyMesh;
    public Sublevel parentSublevel;

    public void SetupBlock(int _depth,int _x, int _y)
    {
        parentSublevel = LevelManager.Instance.sublevelsList[_depth];
        isWalkable = true;
        sublevelPosition = new Vector2(_x, _y);        
    }
   
    public override void Bounce()
    {
        MatchManager.Instance.FloorBounced();
    }

    public override void Headbutt()
    {
        MatchManager.Instance.FloorBounced();
    }

    public override void Activate()
    {
        Debug.Log("KEY OBTAINED!!!");
        LevelManager.Instance.currentSublevel.CollectKey(1);
        keyMesh.GetComponent<CapsuleCollider>().enabled = false;
        keyMesh.SetActive(false);
    }

}
