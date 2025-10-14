using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public Camera MenuCamera;
    public float offsetMultiplier = 1f;
    public float smoothTime = .3f;

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;

        if (MenuCamera == null)
            MenuCamera = GameObject.FindGameObjectWithTag("MenuCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 offset = MenuCamera.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0);

        Vector3 cameraOffset =
            (MenuCamera.transform.right * offset.x) +
            (MenuCamera.transform.up * offset.y);

        Vector3 targetPosition = startPosition + (cameraOffset * offsetMultiplier);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}