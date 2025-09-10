using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveSwitchesRequirement : RequirementBase
{
    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;

    public int switchesID;
    public int switchesNeeded;
    public List<SwitchBehaviour> activeSwitches;
    public float lowestTime;

    public override void Initialize()
    {
        activeSwitches.Clear();
        current = 0;
        goal = switchesNeeded;
    }

    public override void UpdateProgress(object eventData)
    {
        if (eventData is ActiveSwitchEvent switchEvent)
        {
            if (switchEvent.switchID == switchesID) //si es del id de mi requirement
            {
                if (switchEvent.isActive)//ha sido apretado
                {
                    current++;
                    activeSwitches.Add(switchEvent.switchBehaviour);
                    RefreshActiveSwitches();
                }
                else //llega false xq se desactivo
                {
                    current--;
                    activeSwitches.Remove(switchEvent.switchBehaviour);
                }
            
            }
            //Debug.Log($"SW{switchesID}: {current}/{goal}");
        }
    }

    private void RefreshActiveSwitches()
    {
        Debug.Log(activeSwitches.Count);
        foreach (SwitchBehaviour _switch in activeSwitches)
        {
            _switch.elapsedTime = 0;
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}
