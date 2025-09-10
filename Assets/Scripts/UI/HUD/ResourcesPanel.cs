using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesPanel : MonoBehaviour
{
    [SerializeField]
    private Dictionary<ResourceData, ResourceIndicator> indicators = new Dictionary<ResourceData, ResourceIndicator>();
    public GameObject resourceIndicatorPrefab;

    private void Start()
    {
        ClearPanelContent();
        SetupAllIndicators();
    }
    private void OnEnable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged += UpdateIndicators;
        UpdateIndicators();
    }


    private void OnDisable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged -= UpdateIndicators;
    }
    private void UpdateIndicators()
    {
        foreach (var kvp in indicators)
        {
            ResourceData res = kvp.Key;
            ResourceIndicator indicator = kvp.Value;

            int amount = 0;
            ResourceManager.Instance.ownedResources.TryGetValue(res, out amount);

            indicator.UpdateUI(amount);
            indicator.gameObject.SetActive(amount > 0);
        }
    }

    private void ClearPanelContent()
    {
        foreach(Transform _child in this.transform) {
        Destroy(_child.gameObject);
        }
    }

    private void SetupAllIndicators()
    {
        // Instanciar todos los indicadores de inicio
        foreach (var res in ResourceManager.Instance.allAvailableResources)
        {
            var go = Instantiate(resourceIndicatorPrefab, this.transform);
            var indicator = go.GetComponent<ResourceIndicator>();
            indicator.SetupIndicator(res, 0); // empieza en 0

            indicators.Add(res, indicator);
        }
    }


}
