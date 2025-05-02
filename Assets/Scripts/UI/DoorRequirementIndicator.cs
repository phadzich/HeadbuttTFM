using Mono.Cecil;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorRequirementIndicator : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI text;
    public int requiredResources;
    public int currentResources;
    public ResourceData resourceData;

    public void SetupIndicator(ResourceData _resource, int _required, int _current)
    {
        resourceData = _resource;
        icon.sprite = _resource.icon;
        requiredResources = _required;
        currentResources = _current;
        text.text = $"{_current.ToString()}/{_required.ToString()}";
    }
    public void UpdateIndicator(int _current)
    {

        text.text = $"{_current.ToString()}/{requiredResources.ToString()}";

    }
}
