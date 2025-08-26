using UnityEngine;
public class BillboardUI : MonoBehaviour
{
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Mira hacia la c�mara pero mantiene la "verticalidad"
            transform.forward = mainCamera.transform.forward;
        }
    }
}