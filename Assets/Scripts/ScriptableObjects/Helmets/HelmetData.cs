using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelmetData", menuName = "GameData/HelmetData")]
public class HelmetData : ScriptableObject
{
    [Header("Info and aesthetic")]
    public string id;
    [SerializeField] public HelmetInfo helmetInfo = new HelmetInfo();

    [Header("Effect")]
    public EffectTypeEnum effect;
    public ElementEnum element;
    public bool isOvercharged;

    [Header("Stats")]
    public int durability;
    public float headBForce;
    public int evolution;

}
