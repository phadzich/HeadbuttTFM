using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class HelmetPotionBlock : Block
{
    public GameObject potionMesh;
    public Sublevel parentSublevel;
    public int potionSize;
    float meshSize;

    public void SetupBlock(int _depth,int _x, int _y, int _potionSize)
    {
        parentSublevel = LevelManager.Instance.sublevelsList[_depth];
        isWalkable = true;
        sublevelPosition = new Vector2(_x, _y);
        potionSize = _potionSize;
        meshSize = ((float)_potionSize / 10f) + .2f;
        potionMesh.transform.localScale = new Vector3(meshSize, meshSize, meshSize);
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
        Debug.Log("HELMET POTION OBTAINED");
        HelmetManager.Instance.UseHelmetPotion(potionSize);
        potionMesh.SetActive(false);
        potionMesh.GetComponent<CapsuleCollider>().enabled = false;

    }

}
