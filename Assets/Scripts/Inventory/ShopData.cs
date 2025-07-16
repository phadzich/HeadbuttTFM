using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShopData", menuName = "GameData/ShopData")]
public class ShopData : ScriptableObject
{
    public string shopName;
    public List<ShopItem> shopItems;
}
