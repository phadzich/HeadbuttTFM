using System.Collections.Generic;
using UnityEngine;

public class GateRequirementsUI: MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private RequirementUI requirementPrefab;

    private Dictionary<IRequirement, RequirementUI> visuals = new();

    public void AddRequirement(IRequirement req, int cur, int reqd)
    {

        if (req == null)
        {
            Debug.LogError("[Gate] Requirement is NULL al entrar a AddRequirement!");
            return;
        }

        var ui = Instantiate(requirementPrefab, container);
        ui.Setup(req, cur, reqd);
        visuals[req] = ui;
    }

    public void UpdateRequirement(IRequirement req, int cur, int reqd)
    {
        //Debug.Log("Updating counter gate");
        if (visuals.TryGetValue(req, out var ui))
            ui.SetProgress(cur, reqd);
    }

}