using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Info and aesthetic")]
    public string id;
    [SerializeField] public HelmetInfo helmetInfo = new HelmetInfo();
    [SerializeField] public List<ResourceRequirement> requiredResources;

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
