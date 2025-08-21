using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillBar;


    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Hacer que el frente de la barra mire hacia la cámara
            transform.forward = mainCamera.transform.forward;
        }
    }

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
