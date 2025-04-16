using System.Collections.Generic;
using UnityEngine;

public class DoorRequirementsPanel : MonoBehaviour
{
    public GameObject requirementIndicatorPF;
    public List<DoorRequirementIndicator> indicators;
    
    public void SetupPanel(Dictionary<ResourceData, int> _requirements)
    {
        foreach (var _resource in _requirements) {
            var _newIndicatorObject = Instantiate(requirementIndicatorPF, this.transform);
            var _newIndicatorComponent = _newIndicatorObject.GetComponent<DoorRequirementIndicator>();
            indicators.Add(_newIndicatorComponent);
            _newIndicatorComponent.SetupIndicator(_resource.Key, _resource.Value, ResourceManager.Instance.GetOwnedResourceAmount(_resource.Key));
        }
    }

    public void UpdateIndicators()
    {
        foreach(DoorRequirementIndicator _indicatorComponent in  indicators)
        {
            _indicatorComponent.UpdateIndicator(ResourceManager.Instance.GetOwnedResourceAmount(_indicatorComponent.resourceData));
        }
    }

}
