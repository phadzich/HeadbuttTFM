using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningSublevelConfig", menuName = "GameData/Mining Sublevel")]
public class MiningSublevelConfig : SublevelConfig
{

    public List<ResourceData> resourcesList;
    public int blocksToComplete;
    public List<DamageBlock> dmgBlocksList;
    public int dmgBlocksQty;

    [Header("RANDOMNESS")]
    public float noiseScale = 0.2f;
    public float noiseThreshold = 0.4f; // qué tan "lleno" estará el mapa
    public int borderDepth;


}
