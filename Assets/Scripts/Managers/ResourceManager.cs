using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField]
    public Dictionary<ResourceData, int> ownedResources = new();
    [SerializeField]
    public Dictionary<ResourceData, int> storedResources = new();
    [SerializeField]
    public List<ResourceData> allAvailableResources;
    public Action onOwnedResourcesChanged;

    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("ResourceManager Awake");
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        onOwnedResourcesChanged?.Invoke();
    }

    public void AddResource(ResourceData _resource, int _amount)
    {

        //Si ya contiene una entrada con el recurso, aumenta su cantidad
        if (ownedResources.ContainsKey(_resource))
        {
            ownedResources[_resource] += _amount;
        }
        //Si es la primera vez que se adquiere, se crea la entrada con la cantidad
        else
        {
            ownedResources[_resource] = _amount;
        }
        onOwnedResourcesChanged?.Invoke();
        //Debug.Log($"Added {_amount} {_resource.shortName} to inventory");
    }

    public bool SpendResource(ResourceData _resource, int _amount)
    {
        //Si existe el recurso en el inventario y hay suficiente cantidad
        if (ownedResources.ContainsKey(_resource) && ownedResources[_resource] >= _amount)
        {
            //Gastamos esa cantidad y regresamos TRUE
            ownedResources[_resource] -= _amount;
            Debug.Log($"Spent {_amount} of {_resource.shortName}");
            return true;
        }
        //Si no hay recurso o no hay la cantidad necesaria, regresamos FALSE
        onOwnedResourcesChanged?.Invoke();
        Debug.Log($"NOT ENOUGH RESOURCES!");
        return false;
    }

    public bool DepositResource(ResourceData _resource, int _amount)
    {
        //SI TENEMOS SUFICIENTES RECURSOS PARA DEPOSITAR
        if (ownedResources.ContainsKey(_resource) && ownedResources[_resource] >= _amount)
        {
            if (storedResources.ContainsKey(_resource))
            {
                storedResources[_resource] += _amount;
            }
            else
            {
                storedResources[_resource] = _amount;
            }
            //Quitamos lo depositado del owned
            ownedResources[_resource] -= _amount;
            onOwnedResourcesChanged?.Invoke();
            Debug.Log($"Added {_amount} {_resource.shortName} to Storage");
            return true;
        }
        else
        {
            Debug.Log($"NOT ENOUGH RESOURCES TO DEPOSIT!");
            return false;
        }
    }
    public bool WithdrawResource(ResourceData _resource, int _amount)
    {
        //Si existe el recurso en el storage y hay suficiente cantidad
        if (storedResources.ContainsKey(_resource) && storedResources[_resource] >= _amount)
        {
            //Quitamos esa cantidad, la agregamos al owned y regresamos TRUE
            storedResources[_resource] -= _amount;
            ownedResources[_resource] += _amount;
            onOwnedResourcesChanged?.Invoke();
            Debug.Log($"Withdrew {_amount} of {_resource.shortName} from Storage");
            return true;
        }
        //Si no hay recurso o no hay la cantidad necesaria, regresamos FALSE
        Debug.Log($"NOT ENOUGH RESOURCES TO WITHDRAW!");
        return false;
    }

    public int GetOwnedResourceAmount(ResourceData _resource)
    {
        return ownedResources.ContainsKey(_resource) ? ownedResources[_resource] : 0;
    }

    public int GetStoredResourceAmount(ResourceData _resource)
    {
        return storedResources.ContainsKey(_resource) ? storedResources[_resource] : 0;
    }
}