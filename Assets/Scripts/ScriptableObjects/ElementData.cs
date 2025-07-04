using UnityEngine;

[CreateAssetMenu(fileName = "ElementDataData", menuName = "GameData/ElementData")]
public class ElementData : ScriptableObject
{
    public ElementFamily family;
    public string shortName;
    public Sprite icon;
    public Color color;
}
