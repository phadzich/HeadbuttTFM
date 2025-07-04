using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorBlock : Block
{
    public SublevelGoalType currentGoalType;
    public int requiredInt;
    public int currentInt;
    public DoorRequirementIndicator doorRequirementIndicator;
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
        LevelManager.Instance.onSublevelBlocksMined += OnSublevelGoalsAdvanced;
        LevelManager.Instance.onKeysCollected += OnSublevelGoalsAdvanced;
    }

    private void OnDisable()
    {
        LevelManager.Instance.onSublevelBlocksMined -= OnSublevelGoalsAdvanced;
        LevelManager.Instance.onKeysCollected -= OnSublevelGoalsAdvanced;
    }

    public void SetupBlock(int _depth,int _x, int _y,SublevelGoalType _goalType)
    {

        parentSublevel = LevelManager.Instance.sublevelsList[_depth];
        currentGoalType = _goalType;
        UpdateGoals();
        isWalkable = true;
        sublevelPosition = new Vector2(_x, _y);
        doorRequirementIndicator.SetupIndicator(0, requiredInt, currentGoalType);
    }

    public void UpdateGoals()
    {
        switch (currentGoalType)
        {
            case SublevelGoalType.MineBlocks:
                requiredInt = parentSublevel.blocksToComplete;
                currentInt = parentSublevel.currentBlocksMined;
                break;
            case SublevelGoalType.CollectKeys:
                requiredInt = parentSublevel.keysToComplete;
                currentInt = parentSublevel.currentKeysCollected;
                break;
            case SublevelGoalType.Open:
                requiredInt = 0;
                currentInt = 0;
                break;
        }

    }

    public void OnSublevelGoalsAdvanced()
    {
        //Debug.Log("MINED HEARD");
        UpdateGoals();
        doorRequirementIndicator.UpdateIndicator(currentInt);
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
        if (currentInt >= requiredInt)
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

    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        TryToOpen();
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        TryToOpen();
    }

    private void TryToOpen()
    {
        if (isOpen)
        {
            Activate();
        }
        else
        {
            MatchManager.Instance.FloorBounced();
        }
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
        doorRequirementIndicator.gameObject.SetActive(false);
    }
}
