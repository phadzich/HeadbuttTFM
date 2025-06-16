using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

[CreateAssetMenu(fileName = "MiningSublevelConfig", menuName = "GameData/Mining Sublevel")]
public class MiningSublevelConfig : SublevelConfig
{
    public Texture2D sublevel2DMap;
    public SublevelGoalType goalType;

    [Header("Mining Goal")]
    public int blocksToMine;

    [Header("Key Goal")]
    public int keysToCollect;

    [Header("Timer Goal")]
    public float timeLimitSeconds;

    [Header("Gates")]
    public List<GateRequirement> gateRequirements;


    public bool IsValidConfig()
    {
        switch (goalType)
        {
            case SublevelGoalType.MineBlocks:
                return blocksToMine > 0;
            case SublevelGoalType.CollectKeys:
                return keysToCollect > 0;
            case SublevelGoalType.BeatTimer:
                return timeLimitSeconds > 0;
            case SublevelGoalType.Open:
                return true;
            default:
                return false;
        }
    }
}

public enum SublevelGoalType
{
    MineBlocks,
    CollectKeys,
    BeatTimer,
    Open
}
