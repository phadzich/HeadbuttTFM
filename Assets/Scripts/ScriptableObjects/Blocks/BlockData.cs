using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "GameData/BlockData")]
public class BlockData : ScriptableObject
{
    [Header("Helmet Info")]
    public string blockID;
    public GameObject prefab;
}
