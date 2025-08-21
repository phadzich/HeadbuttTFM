using UnityEngine;
using UnityEngine.UI;

public class HBEnergyIndicators : MonoBehaviour
{
    [SerializeField] private HBEnergyCounter[] hbCounters;

    public void UpdateHBUI(float _currentHBPoints, float _maxHBPoints)
    {
        float _counterValue = _maxHBPoints / hbCounters.Length;
        float _progress = _currentHBPoints / _counterValue;

        for (int i = 0; i < hbCounters.Length; i++)
        {
            float _fill = Mathf.Clamp01(_progress - i);
            hbCounters[i].UpdateProgress(_fill);
        }
    }

}
