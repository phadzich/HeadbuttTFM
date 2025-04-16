using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningSublevelConfig", menuName = "GameData/Mining Sublevel")]
public class MiningSublevelConfig : SublevelConfig
{

    public List<ResourceData> resourcesList;

    public List<IntResourcePair> doorRequirements = new();

    public Dictionary<ResourceData, int> sublevelRequirements;
    public void Init()
    {
        sublevelRequirements = new();
        foreach (var pair in doorRequirements)
        {
            if (sublevelRequirements.ContainsKey(pair.key))
                Debug.LogWarning($"Duplicate ResourceData key: {pair.key.name}");
            else
                sublevelRequirements.Add(pair.key, pair.value);
        }
    }

    public int GetAmount(ResourceData resource)
    {
        if (sublevelRequirements == null) Init();
        return sublevelRequirements.TryGetValue(resource, out var value) ? value : 0;
    }


}
