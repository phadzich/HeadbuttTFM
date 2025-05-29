using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public List<HelmetBlueprint> blueprints;
    public ResourceRequirement bouncePrice;
    public ResourceRequirement headbuttPrice;

    public Action HelmetUpgraded; //Se lanza cuando un casco ha sido upgradeado

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
        // Revisamos que no este upgredeado al maximo nivel
        if (helmet.canBeUpgraded)
        {
            List<ResourceRequirement> price = helmet.GetPriceForNextLevel();
            bool canSpend = true;


            // Revisamos si tiene los suficientes recursos porque cada upgrade tiene un precio diferente
            foreach (var res in price)
            {
                if (!ResourceManager.Instance.CanSpendResource(res.resource, res.quantity))
                {
                    canSpend = false;
                    break;
                }
            }

            if (canSpend)
            {
                foreach (var res in price)
                {
                    ResourceManager.Instance.SpendResource(res.resource, res.quantity);
                }
                helmet.helmetXP.LevelUp();
                HelmetUpgraded?.Invoke();
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
