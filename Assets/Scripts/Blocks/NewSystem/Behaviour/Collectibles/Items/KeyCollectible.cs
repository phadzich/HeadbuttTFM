using UnityEngine;

public class KeyCollectible : MonoBehaviour, ICollectibleEffect
{
    public void Activate()
    {
        var _keyEvent = new CollectKeyEvent();
        LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_keyEvent);
    }

    public void SetupBlock(string _variant, MapContext _context)
    {
        
    }
}
