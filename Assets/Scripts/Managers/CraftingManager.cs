using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public List<HelmetBlueprint> blueprints;
    public ResourceRequirement bouncePrice;
    public ResourceRequirement headbuttPrice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("HelmetManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<HelmetBlueprint> GetAvailableBlueprints()
    {
        Dictionary<ResourceData, int> playerResources = ResourceManager.Instance.ownedResources;
        List<HelmetBlueprint> availableBlueprints = new();

        foreach (var blueprint in blueprints)
        {
            if (blueprint.CanCraft(playerResources))
            {
                availableBlueprints.Add(blueprint);
            }
        }

        return availableBlueprints;

    }

    private HelmetBlueprint GetHelmetBlueprint(HelmetInstance helmet) {
        foreach (var blueprint in blueprints)
        {
            if (blueprint.resultHelmet == helmet.baseHelmet)
            {
                return blueprint;
            }
        }

        return new HelmetBlueprint();
    }

    //Llamar cuando se quiera upgradear un casco
    public void UpgradeHelmet(HelmetInstance helmet)
    {
        

    }
}
