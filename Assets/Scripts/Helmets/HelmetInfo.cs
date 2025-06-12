using UnityEngine;

[System.Serializable]
public class HelmetInfo
{
    public string name;
    [TextArea]
    public string description;

    //Aesthetic
    public GameObject mesh;
    public Sprite icon;
    public Color color;
    public Rarity rarity;

    public HelmetInfo Copy()
    {
        return new HelmetInfo
        {
            name = this.name,
            description = this.description,
            mesh = this.mesh,
            icon = this.icon,
            color = this.color,
            rarity = this.rarity
        };
    }
}
