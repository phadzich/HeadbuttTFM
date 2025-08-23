using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sublevel : MonoBehaviour { 

    public string id;
    public int depth;
    public bool isActive;
    public SublevelConfig config;
    public List<ISublevelObjective> activeObjectives;
    public List<IRequirement> activeGateRequirements;
    public List<IRequirement> activeChestRequirements;
    public bool allObjectivesCompleted => (activeObjectives?.Count > 0) && activeObjectives.All(o => o.isCompleted);
    public event Action onSublevelObjectivesUpdated;

    public bool isCompleted;

    public HelmetData helmetToDiscover;

    public void SetupSublevel (string _id, int _depth, bool _isActive, SublevelConfig _config)
    {
        this.id = _id;
        this.depth = _depth;
        this.isActive = _isActive;
        this.config = _config;
        this.helmetToDiscover = _config.helmetBPData;

         if(_config is MiningSublevelConfig _miningConfig)
        {
            SetupObjectives(_miningConfig);
            SetupGates(_miningConfig);
        }
    }

    private void SetupObjectives(MiningSublevelConfig _miningConfig)
    {
        // Si no hay objectives, lista vacía
        activeObjectives = _miningConfig.objectives != null
            ? new List<ISublevelObjective>(_miningConfig.objectives)
            : new List<ISublevelObjective>();

        foreach (var obj in activeObjectives)
        {
            obj?.Initialize(); // defensivo, en caso haya un null dentro
        }
    }

    private void SetupGates(MiningSublevelConfig _miningConfig)
    {
        // Si no hay gates, lista vacía
        activeGateRequirements = _miningConfig.gateRequirements != null
            ? new List<IRequirement>(_miningConfig.gateRequirements)
            : new List<IRequirement>();

        for (int i = 0; i < activeGateRequirements.Count; i++)
        {
            activeGateRequirements[i]?.Initialize(); // defensivo contra nulls internos
        }
    }


    public void DispatchObjectiveEvent(object _e)
    {
        foreach (var _obj in activeObjectives)
        {
            _obj.UpdateProgress(_e);
        }

        foreach (var _req in activeGateRequirements)
        {
            _req.UpdateProgress(_e);
        }

        NotifyObjectivesAndRequirementsUpdated();
    }

    public void NotifyObjectivesAndRequirementsUpdated()
    {
        //Debug.Log($"[Sublevel] Raising objectives update, listeners: {onSublevelObjectivesUpdated?.GetInvocationList().Length ?? 0}");
        onSublevelObjectivesUpdated?.Invoke();
    }

    public void CollectBP()
    {
        HelmetManager.Instance.Discover(helmetToDiscover);
    }
}
