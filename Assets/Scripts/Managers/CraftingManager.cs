using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public List<HelmetBlueprint> blueprints;
    public HashSet<HelmetBlueprint> unlockedBlueprints = new HashSet<HelmetBlueprint>();

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
        // PRUEBA PARA PROTOTIPO QUE TODOS ESTEN DESBLOQUEADOS DESDE UN INICIO
        UnlockHelmetBlueprint(blueprints[0]);
        UnlockHelmetBlueprint(blueprints[1]);
        UnlockHelmetBlueprint(blueprints[2]);
    }

    public void UnlockHelmetBlueprint(HelmetBlueprint _helmetBP)
    {
        unlockedBlueprints.Add(_helmetBP);
    }

    public List<HelmetBlueprint> GetUnlockedBlueprintsByElement(ElementData _element)
    {
        List<HelmetBlueprint> blueprintsByElement = new();

        foreach(var blueprint in unlockedBlueprints)
        {
            if(blueprint.resultHelmet.element == _element)
            {
                blueprintsByElement.Add(blueprint);
            }
        }

        return blueprintsByElement;
    }

    public void CreateHelmet(HelmetBlueprint _blueprint)
    {
        // Pagamos el precio de la creacion del casco
        PayResources(_blueprint.requiredResources);

        // Desbloqueamos el casco
        HelmetManager.Instance.UnlockHelmet(_blueprint.resultHelmet);

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
        UpgradeRequirement req = selectedHelmet.GetUpgradeRequirement(selectedHelmet.nextEvolution);

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
