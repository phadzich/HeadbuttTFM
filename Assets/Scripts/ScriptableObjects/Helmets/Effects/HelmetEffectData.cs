using UnityEngine;

public abstract class HelmetEffectData : ScriptableObject
{
    public string effectType;
    public string effectName;
    [TextArea]
    public string description;
    public Sprite effectIcon;
    public int hbPointsUsed;
    public abstract HelmetEffect CreateEffect();
}
