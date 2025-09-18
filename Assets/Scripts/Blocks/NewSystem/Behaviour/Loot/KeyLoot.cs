using UnityEngine;

[System.Serializable]
public class KeyLoot : LootBase
{
    public override Sprite GetIcon() => UIManager.Instance.iconsLibrary.lootKeySprite;
    public override void Claim()
    {
        Debug.Log("CLAIMED KEY");
        var _keyEvent = new CollectKeyEvent();

        LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_keyEvent);

    }
}
