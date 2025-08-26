using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;
using System.Linq;

[CreateAssetMenu(fileName = "MiningSublevelConfig", menuName = "GameData/Mining Sublevel")]
[System.Serializable]
public class MiningSublevelConfig : SublevelConfig
{
    public Texture2D sublevel2DMap;

    [Header("Objectives")]
    [SerializeReference]
    public List<ISublevelObjective> objectives = new List<ISublevelObjective>();


    [Header("Gates")]
    [SerializeReference]
    public List<IRequirement> gateRequirements;

    [Header("Chests")]
    [SerializeReference]
    public List<IRequirement> chestsRequirements;

    [SerializeReference]
    public List<LootBase> chestsLoot;

}
public enum SublevelGoalType
{
    MineBlocks,
    CollectKeys,
    BeatTimer,
    Open
}
