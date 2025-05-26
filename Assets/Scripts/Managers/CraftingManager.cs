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

    public HelmetBlueprint GetHelmetBlueprint(HelmetInstance helmet) {
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

    public bool hasEnoughResourcesToUpgrade(HelmetInstance helmet)
    {
        List<ResourceRequirement> price = GetHelmetBlueprint(helmet).requiredResources;
        int nextLevel = helmet.level + 1;
        foreach (var res in price)
        {
            if (!ResourceManager.Instance.CanSpendResource(res.resource, res.MultiplyByLevel(nextLevel)))
            {
                return false;
            }
        }

        return true;
    }


    public void UpgradeHelmet(HelmetInstance helmet)
    {
        // Revisamos que no este upgredeado al maximo nivel que por ahora es 5 para todos los cascos
        if (helmet.canBeUpgraded)
        {
            List<ResourceRequirement> price = GetHelmetBlueprint(helmet).requiredResources;
            int nextLevel = helmet.level + 1;

            if (hasEnoughResourcesToUpgrade(helmet))
            {
                foreach (var res in price)
                {
                    ResourceManager.Instance.SpendResource(res.resource, res.MultiplyByLevel(nextLevel));
                }
                helmet.upgradeLevel();
                helmet.upgradeHeadbutt(helmet.level); //Por el momento cada upgrade sube 5 rebotes * el nuevo nivel 
                helmet.upgradeJump(helmet.level); //Por el momento cada upgrade sube 3 headbutts * el nuevo nivel
            }
            else
            {
                Debug.Log("NO HAY SUFICIENTES RECURSOS");
            }
        }
        else
        {
            Debug.Log("HELMET LEVEL MAXED OUT");
        }

    }
}
