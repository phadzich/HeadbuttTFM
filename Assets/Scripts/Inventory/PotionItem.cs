using UnityEngine;

[CreateAssetMenu(fileName = "PotionItemData", menuName = "GameData/PotionItemData")]
public class PotionItem : Item
{
    public PotionTypes potionType;
    public int value;
}

public enum PotionTypes
{
    Default,
    Durability,
    HBPoints
}
