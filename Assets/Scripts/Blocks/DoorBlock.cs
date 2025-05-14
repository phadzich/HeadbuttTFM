using Mono.Cecil;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorBlock : Block
{

    public int requiredBlocks;
    public DoorRequirementsPanel requirementsPanelUI;
    public bool isOpen;
    public GameObject doorTrapMesh;
    public Sublevel parentSublevel;

    private void OnEnable()
    {
        LevelManager.Instance.onSublevelBlocksMined += OnSublevelBlocksMined;
    }

    private void OnDisable()
    {
        LevelManager.Instance.onSublevelBlocksMined -= OnSublevelBlocksMined;
    }

    public void SetupBlock(Sublevel _parentSublevel)
    {
        parentSublevel = _parentSublevel;
        requiredBlocks = parentSublevel.blocksToComplete;
        //Debug.Log(requiredResources);
        requirementsPanelUI.SetupPanel(requiredBlocks);
    }

    public void OnSublevelBlocksMined()
    {
        requirementsPanelUI.UpdateIndicators(parentSublevel.currentBlocksMined);
        if (!isOpen)
        {
            if (DoorRequirementsMet())
            {
                isOpen = true;
                Activate();
            }
        }

    }

    public bool DoorRequirementsMet()
    {
        if (parentSublevel.currentBlocksMined == requiredBlocks)
        {
            //Debug.Log($"COMPLETE {parentSublevel.name}: {parentSublevel.currentBlocksMined}/{requiredBlocks}");
            return true;
        }
        //Debug.Log($"falta {parentSublevel.name}: {parentSublevel.currentBlocksMined}/{requiredBlocks}");
        return false;
    }

    public override void Bounce()
    {

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
