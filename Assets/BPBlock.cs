using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class BPBlock : Block
{
    public int bpID;
    public GameObject bpMesh;
    public Sublevel parentSublevel;
    private HelmetData helmetData;

    public void SetupBlock(int _depth,int _x, int _y, HelmetData _helmData)
    {
        parentSublevel = LevelManager.Instance.sublevelsList[_depth];
        isWalkable = true;
        sublevelPosition = new Vector2(_x, _y);
        helmetData = _helmData;

        if (isAlreadyDiscovered())
        {
            DeactivateBP();
        }
    }

    private bool isAlreadyDiscovered()
    {
        return HelmetManager.Instance.GetInstanceFromData(helmetData).isDiscovered;
    }
   
    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public override void Activate()
    {
        Debug.Log("BP OBTAINED!!!");
        LevelManager.Instance.currentSublevel.CollectBP();
        DeactivateBP();
    }

    private void DeactivateBP()
    {
        bpMesh.GetComponent<CapsuleCollider>().enabled = false;
        bpMesh.SetActive(false);
    }

}
