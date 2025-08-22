using System.Collections.Generic;
using UnityEngine;

public class DoorObjectivesUI: MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private RequirementUI requirementPrefab;

    private Dictionary<ISublevelObjective, RequirementUI> visuals = new();

    public void AddObjective(ISublevelObjective obj, int cur, int reqd)
    {

        if (obj == null)
        {
            return;
        }

        var ui = Instantiate(requirementPrefab, container);
        ui.SetupObjective(obj, cur, reqd);
        visuals[obj] = ui;
    }

    public void UpdateRequirement(ISublevelObjective obj, int cur, int reqd)
    {
        //Debug.Log("Updating counter gate");
        if (visuals.TryGetValue(obj, out var ui))
            ui.SetProgress(cur, reqd);
    }

}