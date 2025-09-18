using UnityEngine;

[CreateAssetMenu(menuName = "GameData/LootIconsLibrary")]
public class IconsLibrary : ScriptableObject
{
    public Sprite defaultSprite;

    [Header("LOOT")]
    public Sprite lootKeySprite;

    [Header("HUD")]
    public Sprite coinSprite;

    [Header("NPCs")]
    public Sprite npcShop;
    public Sprite npcForger;
    public Sprite npcElevator;
    public Sprite npcInventory;
    public Sprite npcRacks;

    [Header("REQUIREMENTS")]
    public Sprite blockReq;
    public Sprite enemyReq;
    public Sprite keyReq;
    public Sprite switch01Req;
    public Sprite switch02Req;
    public Sprite switch03Req;
}