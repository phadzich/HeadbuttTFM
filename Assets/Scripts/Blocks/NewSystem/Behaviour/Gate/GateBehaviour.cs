using System;
using System.Linq;
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

    private int gateID;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        mapContext.sublevel.onSublevelObjectivesUpdated -= CheckRequirements;
    }

    public void SetupBlock(MapContext _context, IRequirement _requirement, int _id)
    {
        GetComponent<BlockNS>().isWalkable = false;
        mapContext = _context;
        gateID = _id;
        Debug.Log($"[SetupBlock] Gate {name} initialized with gateID={gateID} in {_context.sublevel}", this);
        //Debug.Log(gateID);
        mapContext.sublevel.onSublevelObjectivesUpdated += CheckRequirements;
        CheckRequirements();
    }

    public void CheckRequirements()
    {
        //Debug.Log($"[Gate {gateID}] Running CheckObjectives with {mapContext.sublevel.activeGateRequirements.Count} requirements", this);
        if (isOpen) return; // ya abierto, no hace falta chequear
        if (mapContext?.sublevel == null) return;

        var myReqs = mapContext.sublevel.activeGateRequirements
            .Where(r => r.targetId == gateID)
            .ToList();

        //Debug.Log(myReqs.Count);

        if (myReqs.Count == 0) return;
        //Debug.Log($"THIS Gate component: {name}, gateID = {gateID}", this);
        bool allCompleted = true;
        foreach (var req in myReqs)
        {
            Debug.Log($"[Gate {gateID}] Req {req.GetType().Name} Progress {req.current}/{req.goal}");
            if (!req.isCompleted) allCompleted = false;
        }

        if (allCompleted)
        {
            isOpen = true;
            IndicateOpen();
            //Debug.Log($"[Gate {gateID}] OPENED (all requirements completed)");
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
        CheckRequirements();
    }

    private void ResetRequirements()
    {
        foreach (var req in mapContext.sublevel.activeGateRequirements)
        {
            req.Reset();
        }
    }

    public void StopBehaviour()
    {
    }
}
