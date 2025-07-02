using UnityEngine;

public abstract class HelmetEffectData : ScriptableObject
{
    public string effectName;
    [TextArea]
    public string description;

    public abstract HelmetEffect CreateEffect();
}
