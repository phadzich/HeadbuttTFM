using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SwitchSetup))]
public class SwitchBehaviour : MonoBehaviour, IBlockBehaviour
{
    public MapContext mapContext;
    [SerializeField] float duration;
    public bool isActive;
    public GameObject buttonMesh;
    public GameObject shapeMesh;
    public Sublevel parentSublevel;
    public MeshFilter meshFilter; 
    public MeshRenderer meshRenderer;
    public List<Material> materialList;
    public List<Mesh> shapeMeshes;
    [SerializeField]private int switchID;

    public void SetupBlock(MapContext _context, int _id)
    {
        GetComponent<BlockNS>().isWalkable = true;
        mapContext = _context;
        switchID = _id;
        ChangeMeshByID();
        GetDurationFromSublevel();
    }

    private void ChangeMeshByID()
    {
        meshFilter.mesh = shapeMeshes[switchID];
        meshRenderer.material = materialList[switchID];
    }

    private void GetDurationFromSublevel()
    {
        for (int i = 0; i < mapContext.sublevel.activeSwitchesDurations.Count; i++)
        {
            if (i == switchID)
            {
                duration = mapContext.sublevel.activeSwitchesDurations[i];
                break;
            }
        }
    }

    public void IndicateActive()
    {
        GetComponent<BlockNS>().isWalkable = true;
        shapeMesh.SetActive(true);
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
