using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class GateBlock : Block
{
    public GateRequirementIndicator requirementsPanelUI;
    public bool isOpen;
    public bool isActive;
    public GameObject gatesMesh;
    public Sublevel parentSublevel;

    public int gateID;
    public ResourceData requiredResource;
    public int startingAmount;
    public int requiredAmount;
    public int currentAmount;

    private void OnEnable()
    {

        ResourceManager.Instance.onOwnedResourcesChanged += OnOwnedResourcesChanged;
    }

    private void OnDisable()
    {
        ResourceManager.Instance.onOwnedResourcesChanged -= OnOwnedResourcesChanged;
    }

    public void SetupBlock(int _depth,int _x, int _y,int _gateID, ResourceData _reqResource, int _reqAmount)
    {
        parentSublevel = LevelManager.Instance.sublevelsList[_depth];
        isWalkable = false;
        sublevelPosition = new Vector2(_x, _y);
        gateID = _gateID;
        requiredResource = _reqResource;
        requiredAmount = _reqAmount;
        currentAmount = 0;
        ColorGates(_reqResource.mesh.GetComponent<MeshRenderer>().sharedMaterial);
        
    }

    private void ColorGates(Material _mat)
    {
        foreach(Transform _childMesh in gatesMesh.transform)
        {
            _childMesh.gameObject.GetComponent<MeshRenderer>().material = _mat;
        }
    }
    public void StartGateCount()
    {
        if (!ResourceManager.Instance.ownedResources.ContainsKey(requiredResource))
        {
            ResourceManager.Instance.AddResource(requiredResource, 0);

        }
        startingAmount = ResourceManager.Instance.ownedResources[requiredResource];
        isActive = true;
        requirementsPanelUI.SetupIndicator(requiredAmount, 0, requiredResource.icon);
    }

    public void OnOwnedResourcesChanged()
    {
        if (isActive&& ResourceManager.Instance.ownedResources.ContainsKey(requiredResource))
        {
            //Debug.Log("Active and resource exists");
            var _updatedAmount = ResourceManager.Instance.ownedResources[requiredResource];
            //Debug.Log(_updatedAmount);
            currentAmount = ResourceManager.Instance.ownedResources[requiredResource] - startingAmount;
        }

        //Debug.Log("MINED HEARD");
        requirementsPanelUI.UpdateIndicator(currentAmount);

        if (!isOpen)
        {
            //Debug.Log("NOT OPEN");
            if (GateRequirementsMet())
            {
                IndicateOpen();
                isOpen = true;
            }
        }

    }

    public bool GateRequirementsMet()
    {
        if (currentAmount >= requiredAmount)
        {
            Debug.Log("GATE MET");
            return true;
        }

        return false;
    }

    public void IndicateOpen()
    {
        isWalkable = true;
        gatesMesh.SetActive(false);
        requirementsPanelUI.gameObject.SetActive(false);
    }


    public override void Bounce()
    {
        if (isOpen)
        {
            Activate();
        }
        }

    public override void Headbutt()
    {

    }

    public override void Activate()
    {
        //Debug.Log("OPEN DOOOOOOOR");
        //AnimateOpenDoor();
        //this.GetComponent<BoxCollider>().enabled = false;
    }

    private void AnimateOpenDoor()
    {
        //Tween.LocalRotation(doorTrapMesh.transform, endValue: new Vector3(110, 0, 0), duration:1f, ease:Ease.InOutBack);
        //requirementsPanelUI.gameObject.SetActive(false);
    }
}
