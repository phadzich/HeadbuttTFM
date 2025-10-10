using UnityEngine;

[CreateAssetMenu(menuName = "GameData/LootIconsLibrary")]
public class IconsLibrary : ScriptableObject
{
    public Sprite defaultSprite;

    [Header("LOOT")]
    public Sprite lootKeySprite;
    public Sprite HBPotion;
    public Sprite DURPotion;

    [Header("HUD")]
    public Sprite coinSprite;
    public Sprite savedGame;
    public Sprite objectivesComplete;
    public Sprite helmetBroken;

    [Header("NPCs")]
    public Sprite npcShop;
    public Sprite npcForger;
    public Sprite npcElevator;
    public Sprite npcInventory;


    [Header("REQUIREMENTS")]
    public Sprite blockReq;
    public Sprite enemyReq;
    public Sprite keyReq;
    public Sprite switch01Req;
    public Sprite switch02Req;
    public Sprite switch03Req;

    [Header("LOG")]
    public Sprite chestLog;
    public Sprite gateLog;
}