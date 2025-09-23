using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinTrader : MonoBehaviour
{
    Dictionary<ResourceData, float> CoinExchangeRates;
    public int currentCoins;
    public float resourceDifference = 1.5f;
    public float economyScale = 80f;

    //HARDCODED, LUEGO EN UI EVENTS
    public TextMeshProUGUI upgradePointsTXT;
    public Action<int> onCoinsChanged;
    private void Start()
    {
        GenerateExchangeRates();
        onCoinsChanged?.Invoke(currentCoins);
    }
    public void AddCoins(int _amount)
    {
        currentCoins += _amount;
        //upgradePointsTXT.text = currentCoins.ToString();
        CombatLogHUD.Instance.AddLog(UIManager.Instance.iconsLibrary.coinSprite, $"Found <b>{_amount} COINS</b>!");
        onCoinsChanged?.Invoke(currentCoins);
    }

    public bool CanSpendCoins(int _amount)
    {
        bool _result = false;
        if (_amount <= currentCoins)
        {
            _result = true;
            currentCoins -= _amount;
            //upgradePointsTXT.text = currentCoins.ToString();
            onCoinsChanged?.Invoke(currentCoins);
        }
        else
        {
            _result = false;
            Debug.Log("Not enough COINS");
        }
        return _result;
    }

    public void GenerateExchangeRates()
    {
        CoinExchangeRates = new Dictionary<ResourceData, float>();

        foreach (ResourceData _res in ResourceManager.Instance.allAvailableResources)
        {
            // hardness alto = rate alto = menos recursos por moneda
            float _rate = Mathf.Pow(_res.hardness, resourceDifference) / economyScale;
            CoinExchangeRates.Add(_res, _rate);
        }
    }

    public int ResourcesNeededForCoin(ResourceData _resource)
    {
        int _result = 0;

        float _exchangeRate = CoinExchangeRates[_resource];
        float _floatResult = 1 / _exchangeRate;
        _result = Mathf.RoundToInt(_floatResult);
        return _result;
    }

    public void BuyCoin(int _coinAmount, ResourceData _res,int _resAmount)
    {
        if (ResourceManager.Instance.CanSpendResource(_res, _resAmount))
        {
            AddCoins(_coinAmount);
            ResourceManager.Instance.SpendResource(_res, _resAmount);
        }
    }

}
