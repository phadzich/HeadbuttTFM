using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Info and aesthetic")]
    public string id;
    [SerializeField] public HelmetInfo helmetInfo;

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
}
