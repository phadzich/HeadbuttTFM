using UnityEngine;

[System.Serializable]
public class CoinReward : LootBase
{
    public int amount;
    public override void Claim() => Debug.Log("CLAIMED COINS");
}
