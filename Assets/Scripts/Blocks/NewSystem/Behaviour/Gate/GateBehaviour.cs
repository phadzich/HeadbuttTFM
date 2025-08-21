using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GateSetup))]
public class GateBehaviour : MonoBehaviour, IBlockBehaviour
{
    public MapContext mapContext;
    public bool isOpen;
    public bool isActive;
    public GameObject gatesMesh;
    public Sublevel parentSublevel;
    public GateRequirementsUI gateReqsUI;

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

        mapContext.sublevel.onSublevelObjectivesUpdated += CheckRequirements;

        // inicializar requirements una sola vez aquí
        InitializeRequirements();
        CheckRequirements();
    }

    private void InitializeRequirements()
    {
        if (mapContext?.sublevel == null) return;

        var myReqs = mapContext.sublevel.activeGateRequirements
            .Where(r => r.targetId == gateID)
            .ToList();

        foreach (var req in myReqs)
        {
            // suscribirse solo una vez
            req.OnProgressChanged += (cur, reqd) => gateReqsUI.UpdateRequirement(req, cur, reqd);

            // crear UI solo una vez
            gateReqsUI.AddRequirement(req, req.current, req.goal);
        }
    }
    public void CheckRequirements()
    {
        if (isOpen) return;
        if (mapContext?.sublevel == null) return;

        var myReqs = mapContext.sublevel.activeGateRequirements
            .Where(r => r.targetId == gateID)
            .ToList();

        if (myReqs.Count == 0) return;

        bool allCompleted = true;
        foreach (var req in myReqs)
        {
           if (!req.isCompleted) allCompleted = false;
            
        }

        if (allCompleted)
        {
            isOpen = true;
            IndicateOpen();
        }
    }

    public void IndicateOpen()
    {
        GetComponent<BlockNS>().isWalkable = true;
        gatesMesh.SetActive(false);
        gateReqsUI.gameObject.SetActive(false);
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
