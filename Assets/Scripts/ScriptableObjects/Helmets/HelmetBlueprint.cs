using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetBlueprint", menuName = "GameData/HelmetBlueprint")]
public class HelmetBlueprint : ScriptableObject
{
    public string recipeName;
    public HelmetData resultHelmet;
    [SerializeField]
    public List<ResourceRequirement> requiredResources;

    public bool CanCraft(Dictionary<ResourceData, int> playerResources)
    {
        foreach (var requirement in requiredResources)
        {
            if (!playerResources.ContainsKey(requirement.resource) || playerResources[requirement.resource] < requirement.quantity)
            {
                return false;
            }
        }
        return true;

    }

}
