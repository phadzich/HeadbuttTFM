using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Info and aesthetic")]
    public string id;
    [SerializeField] public HelmetInfo helmetInfo = new HelmetInfo();

    [Header("Stats")]
    public int durability;
    public int headbutts;
    public float bounceHeight;
    public int headBForce;
    public float headBCooldown;
    public int knockbackChance;

    [Header("XP")]
    public int baseXP;
    public float xpMultiplier;

    public float GetBaseValue(HelmetStatTypeEnum stat)
    {
        switch (stat)
        {
            case HelmetStatTypeEnum.Durability: return durability;
            case HelmetStatTypeEnum.Headbutts: return headbutts;
            case HelmetStatTypeEnum.BounceHeight: return bounceHeight;
            case HelmetStatTypeEnum.HeadBForce: return headBForce;
            case HelmetStatTypeEnum.HeadBCooldown: return headBCooldown;
            case HelmetStatTypeEnum.KnockbackChance: return knockbackChance;
            default: return 0f;
        }
    }
}
