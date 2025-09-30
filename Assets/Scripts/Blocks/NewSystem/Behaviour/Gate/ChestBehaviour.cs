using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ChestSetup))]
public class ChestBehaviour : MonoBehaviour, IBlockBehaviour
{
    public MapContext mapContext;
    public bool isOpen;
    public bool isClaimed;
    public GameObject doorMesh;
    public GameObject bodyMesh;
    public LootPopupUI lootPopup;
    public ListRequirementsUI chestReqsUI;
    private List<IRequirement> myReqs = new();
    private List<LootBase> myLoot = new();
    private Dictionary<IRequirement, Action<int, int>> handlers = new();

    private int chestID;

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
        chestID = _id;
        isClaimed = false;

        mapContext.sublevel.onSublevelObjectivesUpdated += CheckRequirements;

        InitializeRequirements();
        GetLootFromConfig();
        CheckRequirements();
        lootPopup.Hide();
    }

    private void GetLootFromConfig()
    {
        if (mapContext?.sublevel == null) return;

        // cargar y cachear los rewards
        myLoot = mapContext.sublevel.activeChestRewards
            .Where(r => r.targetId == chestID)
            .ToList();

    }

    private void InitializeRequirements()
    {
        if (mapContext?.sublevel == null) return;

        // cargar y cachear los reqs
        myReqs = mapContext.sublevel.activeChestRequirements
            .Where(r => r.targetId == chestID)
            .ToList();

        foreach (var req in myReqs)
        {
            if (handlers.ContainsKey(req)) continue;

            Action<int, int> handler = (cur, reqd) => {
                chestReqsUI.UpdateRequirement(req, cur, reqd);
            };

            req.OnProgressChanged += handler;
            handlers[req] = handler;

            // crear UI
            chestReqsUI.AddRequirement(req, req.current, req.goal);
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
            GetComponent<BlockNS>().isWalkable = true;
            IndicateOpen();
        }
    }

    public void IndicateOpen()
    {
        GetComponent<BlockNS>().isWalkable = true;
        doorMesh.SetActive(false);
        chestReqsUI.gameObject.SetActive(false);
    }
    public void OnBounced(HelmetInstance _helmetInstance)
    {
        TryClaim();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        TryClaim();
    }

    private void TryClaim()
    {
        if (!isClaimed) //AUN NO SE RECLAMA, reclamar
        {
            ClaimRewards();
        }
        else // YA SE RECLAMo, damage
        {
            MatchManager.Instance.FloorBounced();
        }
    }

    private void ClaimRewards()
    {
        SoundManager.PlaySound(SFXType.OPENCHEST);
        foreach (ILoot _loot in myLoot)
        {
            _loot.Claim();
        }
        isClaimed = true;
        bodyMesh.SetActive(false);

        lootPopup.ShowLoot(myLoot);
    }

    public void Activate()
    {
        
    }

    public void StartBehaviour()
    {
        CheckRequirements();
    }

    public void StopBehaviour()
    {
    }
}
