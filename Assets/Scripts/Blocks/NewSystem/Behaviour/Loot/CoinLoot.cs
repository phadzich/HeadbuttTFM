using UnityEngine;

[System.Serializable]
public class CoinLoot : LootBase
{
    public override Sprite GetIcon() => UIManager.Instance.iconsLibrary.coinSprite;
    public override void Claim()
    {
        ResourceManager.Instance.coinTrader.AddCoins(amount);
    }
}
