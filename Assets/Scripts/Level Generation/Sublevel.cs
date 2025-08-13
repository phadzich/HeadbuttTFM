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
    public bool allObjectivesCompleted => (activeObjectives?.Count > 0) && activeObjectives.All(o => o.isCompleted);
    public event Action onSublevelObjectivesUpdated;

    public bool isCompleted;

    public List<GateBehaviour> gateBlocks = new List<GateBehaviour>();

    public HelmetData helmetToDiscover;

    public void SetupSublevel (string _id, int _depth, bool _isActive, SublevelConfig _config)
    {
        this.id = _id;
        this.depth = _depth;
        this.isActive = _isActive;
        this.config = _config;
        this.helmetToDiscover = _config.helmetBPData;
         if(_config is MiningSublevelConfig)
        {
            SetupObjectives(_config as MiningSublevelConfig);

        }


    }

    private void SetupObjectives(MiningSublevelConfig _miningConfig)
    {
        activeObjectives = new List<ISublevelObjective>(_miningConfig.objectives);

        foreach (var _obj in activeObjectives)
            _obj.Initialize();
    }


    public void DispatchObjectiveEvent(object _e)
    {
        foreach (var _obj in activeObjectives)
        {
            _obj.UpdateProgress(_e);
        }

        NotifyObjectiveUpdated();
    }

    public void NotifyObjectiveUpdated()
    {
        onSublevelObjectivesUpdated?.Invoke();
    }
    public void CollectBP()
    {
        HelmetManager.Instance.Discover(helmetToDiscover);
    }
}
