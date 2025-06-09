using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetBlueprint", menuName = "GameData/HelmetBlueprint")]
public class HelmetBlueprint : ScriptableObject
{
    [SerializeField] public HelmetInfo helmetInfo = new HelmetInfo();
    public EffectTypeEnum effect;
    public ElementEnum element;
    public bool isOvercharged;
    public int baseXP;
    public float xpMultiplier;
    [SerializeField] public List<ResourceRequirement> requiredResources;

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
