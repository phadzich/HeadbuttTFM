using Mono.Cecil;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorBlock : Block
{

    public int requiredBlocks;
    public DoorRequirementsPanel requirementsPanelUI;
    public bool isOpen;
    public GameObject doorTrapMesh;
    public Sublevel parentSublevel;

    public Material openMaterial;

    public GameObject borde1;
    public GameObject borde2;
    public GameObject borde3;
    public GameObject borde4;

    private void OnEnable()
    {
        LevelManager.Instance.onSublevelBlocksMined += OnSublevelBlocksMined;
    }

    private void OnDisable()
    {
        LevelManager.Instance.onSublevelBlocksMined -= OnSublevelBlocksMined;
    }

    public void SetupBlock(int _depth,int _x, int _y)
    {
        parentSublevel = LevelManager.Instance.sublevelsList[_depth];
        requiredBlocks = parentSublevel.blocksToComplete;
        isWalkable = true;
        sublevelPosition = new Vector2(_x, _y);
        requirementsPanelUI.SetupPanel(requiredBlocks);
    }

    public void OnSublevelBlocksMined()
    {
        //Debug.Log("MINED HEARD");
        requirementsPanelUI.UpdateIndicators(parentSublevel.currentBlocksMined);
        if (!isOpen)
        {
            //Debug.Log("NOT OPEN");
            if (DoorRequirementsMet())
            {
                IndicateOpen();
                isOpen = true;
            }
        }

    }

    public bool DoorRequirementsMet()
    {
        if (parentSublevel.currentBlocksMined >= requiredBlocks)
        {
            //Debug.Log("DOOR MET");
            return true;
        }

        return false;
    }

    private void IndicateOpen()
    {
        borde1.GetComponent<MeshRenderer>().material = openMaterial;
        borde2.GetComponent<MeshRenderer>().material = openMaterial;
        borde3.GetComponent<MeshRenderer>().material = openMaterial;
        borde4.GetComponent<MeshRenderer>().material = openMaterial;
    }

    public override void Bounce()
    {
        if (isOpen)
        {
            Activate();
        }
        }

    public override void Headbutt()
    {

    }

    public override void Activate()
    {
        //Debug.Log("OPEN DOOOOOOOR");
        AnimateOpenDoor();
        this.GetComponent<BoxCollider>().enabled = false;
    }

    private void AnimateOpenDoor()
    {
        Tween.LocalRotation(doorTrapMesh.transform, endValue: new Vector3(110, 0, 0), duration:1f, ease:Ease.InOutBack);
        requirementsPanelUI.gameObject.SetActive(false);
    }
}
