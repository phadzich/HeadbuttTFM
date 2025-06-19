using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Info and aesthetic")]
    public string id;
    [SerializeField] public HelmetInfo helmetInfo = new HelmetInfo();
    public ElementEnum element;

    [Header("Stats")]
    public int durability;
    public float headBForce;
    public int evolution;

    [Header("Effects")]
    public EffectTypeEnum effect;
    public OverchargeEffectEnum overchargeEffect;

    [Header("Level up")]
    public UpgradeRequirement[] upgradeRequirements;
}
