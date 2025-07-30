using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCSublevelConfig", menuName = "GameData/NPC Sublevel")]
public class NPCSublevelConfig : SublevelConfig
{
    public NPCType NPCtype;
    public Texture2D sublevel2DMap;
    public int npcShopId;
    public enum NPCType
    {
        Chest,
        Blacksmith,
        Graveyard
    }

}
