using UnityEngine;

public class KeyCollectible : MonoBehaviour, ICollectibleEffect
{
    public void Activate()
    {
        SoundManager.PlaySound(SFXType.COLLECTKEY);
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.keyReq, "<b>Key</b> collected");

        var _keyEvent = new CollectKeyEvent();
        LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_keyEvent);

    }

    public void SetupBlock(string _variant, MapContext _context)
    {
        
    }
}
