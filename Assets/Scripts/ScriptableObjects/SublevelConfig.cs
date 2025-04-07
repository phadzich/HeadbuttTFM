using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SublevelConfig", menuName = "GameData/SublevelConfig")]
public class SublevelConfig : ScriptableObject
{
    public string id;
    public int width;
    public int height;

    public List<ResourceData> resourcesList;

}
