using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesPanel : MonoBehaviour
{
    [SerializeField]
    private List<ResourceIndicator> indicators;
    public GameObject resourceIndicatorPrefab;
    private void OnEnable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged += UpdateIndicators;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged -= UpdateIndicators;
    }
    public void UpdateIndicators()
    {
        ClearPanelContent();
        foreach(KeyValuePair<ResourceData, int> _activeResource in ResourceManager.Instance.ownedResources)
        {
            var _indicator = Instantiate(resourceIndicatorPrefab, this.transform);
            _indicator.GetComponent<ResourceIndicator>().SetupIndicator(_activeResource.Key,_activeResource.Value);
        }
    }

    private void ClearPanelContent()
    {
        foreach(Transform _child in this.transform) {
        Destroy(_child.gameObject);
        }
    }


}
