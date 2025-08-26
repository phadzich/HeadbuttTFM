using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillBar;

    public void UpdateBar(float _curr, float _max)
    {
        if (_curr <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            float _progress = _curr / _max;
            fillBar.fillAmount = _progress;
        }        
    }
}
