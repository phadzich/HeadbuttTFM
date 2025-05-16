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
        // Revisamos que no este upgredeado al maximo nivel que por ahora es 5 para todos los cascos
        if (helmet.canBeUpgraded)
        {
            List<ResourceRequirement> price = GetHelmetBlueprint(helmet).requiredResources;
            bool canSpend = true;
            int nextLevel = helmet.level + 1;

            // Revisamos si tiene los suficientes recursos porque cada upgrade cuesta el precio original del casco * el nivel al que quiere upgreadearlo
            foreach (var res in price)
            {
                if(!ResourceManager.Instance.CanSpendResource(res.resource, res.MultiplyByLevel(nextLevel)))
                {
                    canSpend = false;
                    break;
                }
            }

            if (canSpend)
            {
                foreach (var res in price)
                {
                    ResourceManager.Instance.SpendResource(res.resource, res.MultiplyByLevel(nextLevel));
                }
                helmet.upgradeLevel();
                helmet.upgradeHeadbutt(5 * helmet.level); //Por el momento cada upgrade sube 5 rebotes * el nuevo nivel 
                helmet.upgradeJump(3 * helmet.level); //Por el momento cada upgrade sube 3 headbutts * el nuevo nivel
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
