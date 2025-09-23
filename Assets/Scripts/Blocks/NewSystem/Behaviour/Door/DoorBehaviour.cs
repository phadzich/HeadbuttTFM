using PrimeTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorSetup))]
public class DoorBehaviour : MonoBehaviour, IBlockBehaviour
{

    //public SublevelGoalType currentGoalType;
    //public int requiredInt;
    //public int currentInt;
    public DoorObjectivesUI doorObjectivesUI;
    public bool isOpen;
    public GameObject doorTrapMesh;
    public MapContext mapContext;
    public DoorTrigger doorTrigger;
    //public Sublevel parentSublevel;

    public Material openMaterial;

    public GameObject borde1;
    public GameObject borde2;
    public GameObject borde3;
    public GameObject borde4;
    private Dictionary<ISublevelObjective, Action<int, int>> handlers = new();
    private void OnDisable()
    {
        mapContext.sublevel.onSublevelObjectivesUpdated -= CheckObjectives;
    }

    public void SetupBlock(MapContext _context)
    {
        mapContext = _context;
        mapContext.sublevel.onSublevelObjectivesUpdated += CheckObjectives;
        doorTrigger.gameObject.SetActive(false);
        InitializeObjectives();
        CheckObjectives();

    }

    private void InitializeObjectives()
    {
        foreach (var obj in mapContext.sublevel.activeObjectives)
        {
            if (handlers.ContainsKey(obj)) continue;

            // crear handler y guardarlo
            Action<int, int> handler = (cur, reqd) => doorObjectivesUI.UpdateObjective(obj, cur, reqd);
            obj.OnProgressChanged += handler;
            handlers[obj] = handler;

            // crear UI
            doorObjectivesUI.AddObjective(obj, obj.current, obj.goal);
        }
    }

    public void CheckObjectives()
    {
        //Debug.Log("CHECKOBJECTIVES");
        bool allCompleted = mapContext.sublevel.allObjectivesCompleted;
        //doorRequirementIndicator.UpdateIndicator(currentInt);
        if (allCompleted && !isOpen)
        {
            isOpen = true;
            doorTrigger.gameObject.SetActive(true);
            IndicateOpen();

        }
    }

    private void IndicateOpen()
    {
        borde1.GetComponent<MeshRenderer>().material = openMaterial;
        borde2.GetComponent<MeshRenderer>().material = openMaterial;
        borde3.GetComponent<MeshRenderer>().material = openMaterial;
        borde4.GetComponent<MeshRenderer>().material = openMaterial;
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.enemyReq,"The floor door has opened.");
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        TryToOpen();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
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

    public void Activate()
    {
        //Debug.Log("OPEN DOOOOOOOR");
        AnimateOpenDoor();
        this.GetComponent<BoxCollider>().enabled = false;
    }

    private void AnimateOpenDoor()
    {
        Tween.LocalRotation(doorTrapMesh.transform, endValue: new Vector3(110, 0, 0), duration: 1f, ease: Ease.InOutBack);
        doorObjectivesUI.gameObject.SetActive(false);
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }

    private void OnDestroy()
    {
        foreach (var kvp in handlers)
        {
            kvp.Key.OnProgressChanged -= kvp.Value;
        }
        handlers.Clear();
    }
}
