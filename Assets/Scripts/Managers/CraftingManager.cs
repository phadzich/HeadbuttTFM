using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public List<HelmetBlueprint> blueprints;
    public HashSet<HelmetBlueprint> unlockedBlueprints = new HashSet<HelmetBlueprint>();

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

    public void UnlockHelmetBlueprint(HelmetBlueprint _helmetBP)
    {
        unlockedBlueprints.Add(_helmetBP);
    }

    public List<HelmetBlueprint> GetUnlockedBlueprintsByElement(ElementEnum _element)
    {
        List<HelmetBlueprint> blueprintsByElement = new();

        foreach(var blueprint in unlockedBlueprints)
        {
            if(blueprint.element == _element)
            {
                blueprintsByElement.Add(blueprint);
            }
        }

        return blueprintsByElement;
    }

    //Llamar cuando se quiera upgradear un casco
    public void UpgradeHelmet(HelmetInstance _helmet, HelmetBlueprint _blueprint)
    {
        foreach (var res in _blueprint.requiredResources)
        {
            ResourceManager.Instance.SpendResource(res.resource, res.quantity);
        }

        // Actualiza la informacion del casco como el efecto, elemento, xp
        _helmet.Evolve(_blueprint);

        HelmetUpgraded?.Invoke();
    }

    public bool HasEnoughResources(HelmetBlueprint _blueprint) {

        // Revisamos si tiene los suficientes recursos porque cada upgrade tiene un precio diferente
        foreach (var res in _blueprint.requiredResources)
        {
            if (!ResourceManager.Instance.CanSpendResource(res.resource, res.quantity))
            {
                Debug.Log("NO HAY SUFICIENTES RECURSOS");
                return false;
            }
        }

        return true;
    }
}
