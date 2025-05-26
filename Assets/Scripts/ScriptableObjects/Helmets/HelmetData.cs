using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    public string id;
    public string helmetName;
    [TextArea] public string description;

    public int bounces;
    public int headbutts;

    public GameObject mesh;
    public Sprite icon;
    public Color color;

    // Efecto
    public HelmetEffectType effect;
}
