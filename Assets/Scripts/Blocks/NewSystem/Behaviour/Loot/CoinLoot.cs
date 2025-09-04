using Mono.Cecil;
using UnityEngine;

[System.Serializable]
public class CoinLoot : LootBase
{
    public override Sprite GetIcon() => UIManager.Instance.lootIcons.coinSprite;
    public override void Claim()
    {
        Debug.Log("CLAIMED COINS");
        ResourceManager.Instance.coinTrader.AddCoins(amount);
    }
}
