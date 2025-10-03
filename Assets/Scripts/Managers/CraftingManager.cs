using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    public HelmetInstance selectedHelmet;

    public Action<HelmetInstance> HelmetSelected; // Se lanza cuando un casco ha sido seleccionado
    public Action HelmetCrafted; // Se lanza cuando un casco ha sido upgradeado

    private UIType sound;

    public void UpdateSound(UIType _sound) => sound = _sound;

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

    public void Craft()
    {
        if (selectedHelmet == null) return;
        // Pagamos el precio de la creacion del casco
        if (CanCraft(selectedHelmet.GetUpgradeRequirement()))
        {
            SoundManager.PlaySound(sound);
            // Obtenemos los upgrade requirements del casco para su siguiente evolucion
            UpgradeRequirement req = selectedHelmet.GetUpgradeRequirement();
            PayResources(req.requirements);
            // Desbloqueamos el casco
            selectedHelmet.Craft();

            HelmetCrafted?.Invoke();
        }
    }

    public bool CanCraft(UpgradeRequirement _req)
    {
        foreach (var requirement in _req.requirements)
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
        HelmetSelected?.Invoke(_helmet);
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
