using UnityEngine;

[CreateAssetMenu(fileName = "PotionItemData", menuName = "GameData/PotionItemData")]
public class PotionItem : Item
{
    public PotionTypes potionType;


    public override void Use()
    {
        switch (potionType)
        {
            case PotionTypes.Default: 
                break;
            case PotionTypes.Durability:
                HelmetManager.Instance.currentHelmet.HealDurability(value, true); 
                break;
            case PotionTypes.HBPoints:
                PlayerManager.Instance.playerHeadbutt.AddHBPoints(value, true);
                break;
        }
    }


}

public enum PotionTypes
{
    Default,
    Durability,
    HBPoints
}
