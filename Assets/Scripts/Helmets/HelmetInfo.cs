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

}
