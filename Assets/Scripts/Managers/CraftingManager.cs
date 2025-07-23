using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public List<HelmetBlueprint> blueprints;
    public List<HelmetBlueprint> unlockedBlueprints = new List<HelmetBlueprint>();

    public HelmetInstance selectedHelmet;

    public Action HelmetSelected; // Se lanza cuando un casco ha sido seleccionado
    public Action HelmetEvolved; // Se lanza cuando un casco ha sido upgradeado

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

    private void Start()
    {
        /* PRUEBA PARA PROTOTIPO QUE TODOS ESTEN DESBLOQUEADOS DESDE UN INICIO
        UnlockHelmetBlueprint(blueprints[0]);
        UnlockHelmetBlueprint(blueprints[1]);
        UnlockHelmetBlueprint(blueprints[2]);*/
    }

    public void Craft(HelmetInstance _helmet)
    {
        // Pagamos el precio de la creacion del casco
        if (CanCraft(_helmet.baseHelmet))
        {
            PayResources(_helmet.baseHelmet.requiredResources);
            // Desbloqueamos el casco
            _helmet.Craft();
        }
    }

    public bool CanCraft(HelmetData _data)
    {
        foreach (var requirement in _data.requiredResources)
        {
            if (!ResourceManager.Instance.ownedResources.ContainsKey(requirement.resource) || ResourceManager.Instance.ownedResources[requirement.resource] < requirement.quantity)
            {
                return false;
            }
        }
        return true;

    }

    // Funcion para elegir un casco desde la UI
    public void SelectHelmet(HelmetInstance _helmet)
    {
        selectedHelmet = _helmet;
        HelmetSelected?.Invoke();
    }


    //Llamar cuando se quiera upgradear un casco
    public void EvolveHelmet()
    {
        if (selectedHelmet == null) return;

        // Obtenemos los upgrade requirements del casco para su siguiente evolucion
        UpgradeRequirement req = selectedHelmet.GetUpgradeRequirement(selectedHelmet.nextLevel);

        // Pagamos el precio de la evolucion
        PayResources(req.requirements);

        // Actualiza la informacion del casco como el efecto, elemento
        selectedHelmet.Evolve(req);

        HelmetEvolved?.Invoke();
    }

    // Llamar esta funcion para pagar el costo de lo que se haga
    // La verificacion de si hay suficientes recursos se hace en otro lado
    private void PayResources(List<ResourceRequirement> _requirements)
    {
        foreach (var res in _requirements)
        {
            ResourceManager.Instance.SpendResource(res.resource, res.quantity);
        }
    }

}
