using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeRequirement
{
    [Tooltip("Nivel al que quieres subir con estos requisitos")]
    public List<ResourceRequirement> requirements;

    [Header("Stats upgrade")]
    public int durability;
    public float powerStat;
}
