using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetBlueprint", menuName = "GameData/HelmetBlueprint")]
public class HelmetBlueprint : ScriptableObject
{
    public string recipeName;
    public HelmetData resultHelmet;
    public List<ResourceData> requiredResources;
}
