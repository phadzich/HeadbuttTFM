using UnityEngine;

[System.Serializable]
public class ActiveSwitchesRequirement : RequirementBase
{
    [SerializeField] private Sprite icon;
    public override Sprite GetIcon() => icon;

    public int switchesID;
    public int switchesNeeded;

    public override void Initialize()
    {     
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
                }
                else //llega false xq se desactivo
                {
                    current--;
                }
            
            }
            Debug.Log($"{current}/{goal}");
        }
    }

    public override bool isCompleted => current >= goal;
    public override float progress => (float)current / goal;
}
