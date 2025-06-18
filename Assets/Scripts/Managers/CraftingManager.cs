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

    public List<HelmetBlueprint> GetUnlockedBlueprintsByElement(ElementEnum _element)
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

    public List<HelmetBlueprint> GetUnlockedBlueprintsByEvolutionReq(int _evolution)
    {
        List<HelmetBlueprint> blueprintsByRequirement = new();

        foreach (var blueprint in unlockedBlueprints)
        {
            if (blueprint.requiredEvolution == _evolution)
            {
                blueprintsByRequirement.Add(blueprint);
            }
        }

        return blueprintsByRequirement;
    }

    // Funcion para elegir un casco desde la UI
    public void SelectHelmet(HelmetInstance _helmet)
    {
        selectedHelmet = _helmet;
        HelmetSelected?.Invoke();
    }


    //Llamar cuando se quiera upgradear un casco
    public void EvolveHelmet(HelmetBlueprint _blueprint)
    {
        if (selectedHelmet == null) return;

        foreach (var res in _blueprint.requiredResources)
        {
            ResourceManager.Instance.SpendResource(res.resource, res.quantity);
        }

        // Actualiza la informacion del casco como el efecto, elemento
        selectedHelmet.Evolve(_blueprint);

        HelmetEvolved?.Invoke();
    }
}
