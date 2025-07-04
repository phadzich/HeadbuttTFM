using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Info and aesthetic")]
    public string id;
    [SerializeField] public HelmetInfo helmetInfo = new HelmetInfo();

    [Header("Compatibility")]
    public MiningPower miningPower;
    public ElementData element;

    [Header("Stats")]
    public int durability;
    public float headBForce;
    public int evolution;

    [Header("Effects")]
    public List<HelmetEffectData> effects;
    public List<HelmetEffectData> overchargedEffects;

    [Header("Level up")]
    public UpgradeRequirement[] upgradeRequirements;
}
