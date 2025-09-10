using TMPro;
using UnityEngine;

public class CoinsPanelUI : MonoBehaviour
{
    public TextMeshProUGUI coinsTXT;

    private void OnEnable()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        coinsTXT.text = ResourceManager.Instance.coinTrader.currentCoins.ToString();
    }
}
