using UnityEngine;

[CreateAssetMenu(menuName = "GameData/LootIconsLibrary")]
public class IconsLibrary : ScriptableObject
{
    public Sprite defaultSprite;

    [Header("LOOT")]
    public Sprite keySprite;

    [Header("HUD")]
    public Sprite coinSprite;

    [Header("NPCs")]
    public Sprite npcShop;
    public Sprite npcForger;
    public Sprite npcElevator;
    public Sprite npcInventory;
    public Sprite npcRacks;
}