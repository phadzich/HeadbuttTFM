using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningSublevelConfig", menuName = "GameData/Mining Sublevel")]
public class MiningSublevelConfig : SublevelConfig
{
    public int width;
    public int height;

    public List<ResourceData> resourcesList;

}
