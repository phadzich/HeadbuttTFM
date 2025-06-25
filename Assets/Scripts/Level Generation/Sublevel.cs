using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Sublevel : MonoBehaviour { 

    public string id;
    public int depth;
    public bool isActive;
    public SublevelConfig config;

    public bool isCompleted;

    public int blocksToComplete;
    public int currentBlocksMined;

    public int keysToComplete;
    public int currentKeysCollected;

    public float timeToBeat;
    public float currentTime;

    public int maxResourceBlocks;
    public bool isTotallyMined => currentBlocksMined == maxResourceBlocks;

    public List<GateBlock> gateBlocks = new List<GateBlock>();

    public void SetupSublevel (string _id, int _depth, bool _isActive, SublevelConfig _config)
    {
        this.id = _id;
        this.depth = _depth;
        this.isActive = _isActive;
        this.config = _config;
    }

    public void SetMiningObjectives(int _objective)
    {
        blocksToComplete = _objective;
        currentBlocksMined = 0;
    }

    public void SetKeysObjectives(int _objective)
    {
        keysToComplete = _objective;
        currentKeysCollected = 0;
    }

    public void SetTimerObjectives(float _objective)
    {
        timeToBeat = _objective;
        currentTime = 0;
    }

    public void CollectKey(int _amount)
    {
        currentKeysCollected += _amount;
        LevelManager.Instance.onKeysCollected?.Invoke();
    }
}
