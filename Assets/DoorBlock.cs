using Mono.Cecil;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorBlock : Block
{

    public Dictionary<ResourceData, int> requiredResources;
    public DoorRequirementsPanel requirementsPanelUI;
    public bool isOpen;
    public GameObject doorTrapMesh;

    private void OnEnable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged += OnOwnedResourcesChanged;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged -= OnOwnedResourcesChanged;
    }

    public void SetupBlock(Dictionary<ResourceData, int> _requiredResources)
    {
        requiredResources = _requiredResources;
        //Debug.Log(requiredResources);
        requirementsPanelUI.SetupPanel(_requiredResources);
        if (DoorRequirementsMet())
        {
            isOpen = true;
            Activate();
        }
    }

    public void OnOwnedResourcesChanged()
    {
        requirementsPanelUI.UpdateIndicators();
        if (!isOpen)
        {

            if (DoorRequirementsMet())
            {
                isOpen = true;
                Activate();

            }
        }

    }

    public bool DoorRequirementsMet()
    {
        int _requirementsCount = requiredResources.Count;
        int _requirementsCompleted = 0;
        foreach (KeyValuePair<ResourceData, int> _requirement in requiredResources)
        {
            if (ResourceManager.Instance.GetOwnedResourceAmount(_requirement.Key) >= _requirement.Value)
            {
                _requirementsCompleted++;
            }

        }
        if (_requirementsCompleted == _requirementsCount)
        {
            Debug.Log($"Completed {_requirementsCompleted} de {_requirementsCount}");
            return true;
        }
        return false;
    }

    public override void Bounce()
    {

    }

    public override void Headbutt()
    {

    }

    public override void Activate()
    {
        Debug.Log("OPEN DOOOOOOOR");
        AnimateOpenDoor();
        this.GetComponent<BoxCollider>().enabled = false;
    }

    private void AnimateOpenDoor()
    {
        Tween.LocalRotation(doorTrapMesh.transform, endValue: new Vector3(110, 0, 0), duration:1f, ease:Ease.InOutBack);
        requirementsPanelUI.gameObject.SetActive(false);
    }
}
