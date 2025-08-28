using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "LevelTools/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    public ColorToString[] colors;


    void MarkDirty(UnityEngine.Object obj)
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(obj);
#endif
    }
}

public enum PaintCategory { LVL, RESOURCES, NPC, ITEMS, CHESTS, GATES, FIRE, WATER, GRASS, ROCK, ELECTRIC, SWITCHES }

