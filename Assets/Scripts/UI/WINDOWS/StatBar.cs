using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class StatBar : MonoBehaviour
{
    public Slider bar;

    public void SetMaxVal(int _max)
    {
        bar.maxValue = _max;
    }

    public void UpdateBar(float _currentValue)
    {
        bar.value = _currentValue;
    }
}
