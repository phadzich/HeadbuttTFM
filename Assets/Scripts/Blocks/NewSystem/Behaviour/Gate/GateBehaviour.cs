using PrimeTween;
using System;
using System.Collections.Generic;
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
    public ListRequirementsUI gateReqsUI;
    private List<IRequirement> myReqs = new();
    private Dictionary<IRequirement, Action<int, int>> handlers = new();

    public GameObject leds;
    public GameObject lightObject;
    public Material openMat;
    public ParticleSystem openParticles;

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

        // inicializar requirements una sola vez aqu�
        InitializeRequirements();
        CheckRequirements();
    }

    private void InitializeRequirements()
    {
        if (mapContext?.sublevel == null) return;

        // cargar y cachear los reqs
        myReqs = mapContext.sublevel.activeGateRequirements
            .Where(r => r.targetId == gateID)
            .ToList();

        foreach (var req in myReqs)
        {
            if (handlers.ContainsKey(req)) continue;

            // crear handler
            Action<int, int> handler = (cur, reqd) => gateReqsUI.UpdateRequirement(req, cur, reqd);

            req.OnProgressChanged += handler;
            handlers[req] = handler;

            // crear UI
            gateReqsUI.AddRequirement(req, req.current, req.goal);
        }
    }

    private void OnDestroy()
    {
        foreach (var kvp in handlers)
        {
            kvp.Key.OnProgressChanged -= kvp.Value;
        }
        handlers.Clear();
    }

    public void CheckRequirements()
    {
        if (isOpen || myReqs == null || myReqs.Count == 0) return;

        bool allCompleted = true;
        foreach (var req in myReqs)
        {
            if (!req.isCompleted)
            {
                allCompleted = false;
                break; // ni sigas, ya sabes que falta
            }
        }

        if (allCompleted)
        {
            isOpen = true;
            IndicateOpen();
        }
    }

    public void IndicateOpen()
    {
        SoundManager.PlaySound(SFXType.UNLOCK_GATE);
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.gateLog, "A <b>GATE</b> has opened somewhere!");
        GetComponent<BlockNS>().isWalkable = true;
        AnimateSpearsDown();
        gateReqsUI.gameObject.SetActive(false);
        openParticles.Play();
        leds.GetComponent<MeshRenderer>().material = openMat;
        
}

    private void AnimateSpearsDown()
    {
        Tween.LocalPositionY(gatesMesh.transform, endValue: -1f, duration: 1f, ease: Ease.InOutBack).OnComplete(() =>lightObject.SetActive(false));
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
