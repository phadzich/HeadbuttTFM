using System.Collections.Generic;
using UnityEngine;

public class DoorRequirementsPanel : MonoBehaviour
{
    public GameObject requirementIndicatorPF;
    DoorRequirementIndicator requirementIndicator;

    public void SetupPanel(int _requirements)
    {
            var _newIndicatorObject = Instantiate(requirementIndicatorPF, this.transform);
        requirementIndicator = _newIndicatorObject.GetComponent<DoorRequirementIndicator>();
        requirementIndicator.SetupIndicator(_requirements,0);
    }

    public void UpdateIndicators(int _qty)
    {
        requirementIndicator.UpdateIndicator(_qty);

    }

}
