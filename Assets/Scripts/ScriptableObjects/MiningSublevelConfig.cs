using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningSublevelConfig", menuName = "GameData/Mining Sublevel")]
public class MiningSublevelConfig : SublevelConfig
{
    public Texture2D sublevel2DMap;
    public int blocksToComplete;
}
