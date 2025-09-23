using UnityEngine;

public class BPCollectible : MonoBehaviour, ICollectibleEffect
{
    private Sublevel parentSublevel;
    private HelmetData helmetData;

    public void SetupBlock(string _variant, MapContext _context)
    {
        parentSublevel = _context.sublevel;
        helmetData = _context.sublevel.helmetToDiscover;

        if (isAlreadyDiscovered())
        {
            return;
        }
    }

    private bool isAlreadyDiscovered()
    {
        return HelmetManager.Instance.GetInstanceFromData(helmetData).isDiscovered;
    }

    public void Activate()
    {
        CombatLogHUD.Instance.AddLog(helmetData.icon, $"<b>{helmetData.helmetName}</b> helmet discovered!");

        LevelManager.Instance.currentSublevel.CollectBP();
        UIManager.Instance.popupUI.ShowPopup(helmetData.helmetName, "DISCOVERED", helmetData.icon);

    }
}
