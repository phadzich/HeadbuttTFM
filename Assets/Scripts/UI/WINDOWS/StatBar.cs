using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class StatBar : MonoBehaviour
{
    public TextMeshProUGUI statVal;

    public Image[] ticks; // Asignar en el Inspector
    public Color filledColor;
    public Color emptyColor;

    public void UpdateBar(int _upgradeCount, float _currentValue)
    {
        UpdateTicks(_upgradeCount);
        UpdateText(_currentValue);
    }

    private void UpdateTicks(int _upgradeCount)
    {

        for (int i = 0; i < ticks.Length; i++)
        {
            ticks[i].color = i < _upgradeCount ? filledColor : emptyColor;
        }
    }

    private void UpdateText(float _currentValue)
    {
        statVal.text = _currentValue.ToString("0.#"); // Formateado con 1 decimal máx.
    }
}
