using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeRequirement
{
    [Tooltip("Nivel al que quieres subir con estos requisitos")]
    public int toEvolution; 
    public List<ResourceRequirement> requirements;

    [Header("Aesthetic")]
    [SerializeField] public HelmetInfo newInfo = new HelmetInfo();

    [Header("Stats upgrade")]
    public int durabilityAdd;
    public int HBForceAdd;

    [Header("Effects modifiers")]
    public bool activateEffect;
    public bool isOvercharged;
}
