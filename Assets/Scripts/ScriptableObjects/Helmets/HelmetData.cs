using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Info")]
    public string id;
    public string helmetName;
    [TextArea] public string description;

    [Header("Stats")]
    public int durability;
    public int headbutts;
    public HelmetEffectType helmetEffect;

    [Header("XP")]
    public int baseXP;
    public float xpMultiplier;


    [Header("Helmet Appearance")]
    public List<GameObject> meshesByLevel;
    public Sprite icon;
    public Color color;
}
