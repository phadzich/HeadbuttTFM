using UnityEngine;
using UnityEngine.UI;

public class ExitFloatinIndicatorHUD : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;            // El jugador
    public Transform exitDoor;          // La puerta de salida
    public RectTransform arrowUI;       // La flecha en el canvas
    public GameObject arrowObject;
    public Color completedColor;
    public Color incompleteColor;
    public Color incompleteBorderColor;

    [Header("Offset")]
    public float screenMargin = 100f;   // Margen para que la flecha no llegue al borde

    void Update()
    {
            if (player == null || exitDoor == null || arrowUI == null)
                return;

            // Posición de la puerta en pantalla
            Vector3 doorScreenPos = Camera.main.WorldToScreenPoint(exitDoor.position);

            // ¿Está frente a la cámara?
            bool isInFront = doorScreenPos.z > 0;

            // ¿Está en pantalla?
            bool isVisible = isInFront &&
                             doorScreenPos.x >= 0 && doorScreenPos.x <= Screen.width &&
                             doorScreenPos.y >= 0 && doorScreenPos.y <= Screen.height;

            if (isVisible)
        {
            arrowUI.GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            arrowUI.GetComponent<CanvasGroup>().alpha = .8f;
        }

        if (!isVisible)
        {
            // Centro de pantalla
            Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;

            // Dirección desde el centro hasta la puerta
            Vector2 dir = ((Vector2)doorScreenPos - screenCenter).normalized;

            // Posición final, cerca del borde
            float maxX = screenCenter.x - screenMargin;
            float maxY = screenCenter.y - screenMargin;

            float t = Mathf.Min(
                Mathf.Abs(maxX / dir.x),
                Mathf.Abs(maxY / dir.y)
            );

            Vector2 screenPos = screenCenter + dir * t;

            // Convertir a coordenadas locales del canvas
            Canvas canvas = arrowUI.GetComponentInParent<Canvas>();
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPoint);

            arrowUI.anchoredPosition = localPoint;

            // Rotar flecha para que apunte en la dirección correcta
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arrowUI.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (LevelManager.Instance.currentSublevel.allObjectivesCompleted)
        {
            arrowObject.gameObject.GetComponent<Image>().color = completedColor;
            this.GetComponent<Image>().color = completedColor;
        }
        else
        {
            arrowObject.gameObject.GetComponent<Image>().color = incompleteColor;
            this.GetComponent<Image>().color = incompleteBorderColor;
        }
}
}
