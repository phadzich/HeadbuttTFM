using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Helmet Info")]
    public string id;
    public string helmetName;
    [TextArea]
    public string description;
    [SerializeField] public List<ResourceRequirement> requiredResources;

    [Header("Aesthetic")]
    public GameObject mesh;
    public Sprite icon;
    public Color color;
    public Rarity rarity;

    [Header("Compatibility")]
    public MiningPower miningPower;
    public ElementData element;

    [Header("Stats")]
    public int durability;

    [Header("Effects")]
    public List<HelmetEffectData> effects;

    [Header("Level up")]
    public UpgradeRequirement[] levelUpRequirements;
}
