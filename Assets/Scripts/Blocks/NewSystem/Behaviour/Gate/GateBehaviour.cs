using System;
using UnityEngine;

[RequireComponent(typeof(GateSetup))]
public class GateBehaviour : MonoBehaviour, IBlockBehaviour
{
    public GateRequirementIndicator requirementsPanelUI;
    public MapContext mapContext;
    public bool isOpen;
    public bool isActive;
    public GameObject gatesMesh;
    public Sublevel parentSublevel;

    public int gateID;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void SetupBlock(MapContext _context, IGateRequirement _requirement, int _id)
    {
        GetComponent<BlockNS>().isWalkable = false;
        mapContext = _context;
        gateID = _id;
        mapContext.sublevel.onSublevelObjectivesUpdated += CheckObjectives;
        CheckObjectives();

    }

    public void CheckObjectives()
    {
        //Debug.Log("CHECKOBJECTIVES");
        bool allCompleted = mapContext.sublevel.allObjectivesCompleted;
        //doorRequirementIndicator.UpdateIndicator(currentInt);
        if (allCompleted && !isOpen)
        {
            isOpen = true;
            IndicateOpen();
        }
    }

    public void IndicateOpen()
    {
        GetComponent<BlockNS>().isWalkable = true;
        gatesMesh.SetActive(false);
        requirementsPanelUI.gameObject.SetActive(false);
    }


    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();

    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public void Activate()
    {
        
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }
}
