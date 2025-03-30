using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "GameData/ResourceData")]
public class ResourceData : ScriptableObject
{

    public int id;
    public string shortName;
    public int hardness;
    public ResourceRarity rarity;
    public string description;
    public Sprite icon;
    public Color color;
    public GameObject mesh;

    public enum ResourceRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}
