using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceTrader : MonoBehaviour
{
    Dictionary<ResourceData, float> upgradePointsExchangeRates;
    public int upgradePoints;
    public float diferenciaEntreRecursos = 1.5f;
    public float escalaEconomia = 100f;

    //HARDCODED, LUEGO EN UI EVENTS
    public TextMeshProUGUI upgradePointsTXT;

    private void Start()
    {
        GenerateExchangeRates();
    }
    public void AddUpgradePoints(int _amount)
    {
        upgradePoints += _amount;
        upgradePointsTXT.text = upgradePoints.ToString();
    }

    public void SpendUpgradePoints(int _amount)
    {
        if (_amount <= upgradePoints)
        {
            upgradePoints -= _amount;
            upgradePointsTXT.text = upgradePoints.ToString();
        }
        else
        {
            Debug.Log("Not enough Upgrade Points");
        }
    }

    public void GenerateExchangeRates()
    {
        Debug.Log("GENERATING EXCHANGE RATES");
        upgradePointsExchangeRates = new Dictionary<ResourceData, float>();
        int _i = 1;
        foreach (ResourceData _res in ResourceManager.Instance.allAvailableResources)
        {
            _i++;
            float _rate = Mathf.Pow((float)_i, diferenciaEntreRecursos) / escalaEconomia;
            upgradePointsExchangeRates.Add(_res, _rate);
        }
    }

    public int ResourcesNeededForUpgradePoint(ResourceData _resource)
    {
        int _result = 0;

        float _exchangeRate = upgradePointsExchangeRates[_resource];
        Debug.Log(_exchangeRate);
        float _floatResult = 1 / _exchangeRate;
        Debug.Log(_floatResult);
        _result = Mathf.RoundToInt(_floatResult);
        Debug.Log(_result);
        return _result;

    }

    public void BuyUpgradePoint(int _UPamount, ResourceData _res,int _resAmount)
    {
        if (ResourceManager.Instance.CanSpendResource(_res, _resAmount))
        {
            AddUpgradePoints(_UPamount);
            ResourceManager.Instance.SpendResource(_res, _resAmount);
        }
    }

}
