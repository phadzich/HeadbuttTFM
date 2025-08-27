using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "LevelTools/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    public ColorToString[] colors;

}

public enum PaintCategory { LVL, RESOURCES, NPC, ITEMS, CHESTS, GATES, FIRE, WATER, GRASS, ROCK, ELECTRIC, SWITCHES }