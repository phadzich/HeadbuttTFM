using UnityEngine;

[System.Serializable]
public class KeyLoot : LootBase
{
    public override Sprite GetIcon() => UIManager.Instance.iconsLibrary.lootKeySprite;
    public override void Claim()
    {
        var _keyEvent = new CollectKeyEvent();

        LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_keyEvent);
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.lootKeySprite, $"<b>KEY</b> found in CHEST!");
        PlayerManager.Instance.groundAnimations.Play("Objective_Complete");
    }
}
