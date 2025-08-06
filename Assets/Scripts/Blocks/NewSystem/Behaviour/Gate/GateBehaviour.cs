using UnityEngine;

[RequireComponent(typeof(GateSetup))]
public class GateBehaviour : MonoBehaviour, IBlockEffect
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

    public void SetupBlock(Sublevel _sublevel, int _gateID, GateRequirement _gateReq)
    {
        Debug.Log(_gateID);
        parentSublevel = _sublevel;
        gateID = _gateID;
        requiredResource = _gateReq.requiredResource;
        requiredAmount = _gateReq.requiredAmount;
        currentAmount = 0;
        ColorGates(requiredResource.resMesh.GetComponent<MeshRenderer>().sharedMaterial);

    }

    private void ColorGates(Material _mat)
    {
        foreach (Transform _childMesh in gatesMesh.transform)
        {
            _childMesh.gameObject.GetComponent<MeshRenderer>().material = _mat;
        }
    }
    public void StartGateCount()
    {
        startingAmount = ResourceManager.Instance.ownedResources[requiredResource];
        isActive = true;
        requirementsPanelUI.SetupIndicator(requiredAmount, 0, requiredResource.icon);
    }

    public void OnOwnedResourcesChanged()
    {
        if (isActive && ResourceManager.Instance.ownedResources.ContainsKey(requiredResource))
        {
            var _updatedAmount = ResourceManager.Instance.ownedResources[requiredResource];
            currentAmount = ResourceManager.Instance.ownedResources[requiredResource] - startingAmount;
        }

        requirementsPanelUI.UpdateIndicator(currentAmount);

        if (!isOpen)
        {
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
            return true;
        }

        return false;
    }

    public void IndicateOpen()
    {
        GetComponent<BlockNS>().isWalkable = true;
        gatesMesh.SetActive(false);
        requirementsPanelUI.gameObject.SetActive(false);
    }


    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        if (isOpen)
        {
            Activate();
        }
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public void Activate()
    {
        
    }
}
