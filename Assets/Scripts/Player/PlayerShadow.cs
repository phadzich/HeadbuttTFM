using UnityEngine;

public class PlayerShadow : MonoBehaviour
    {
    public Rigidbody rb; // Rigidbody del jugador
    public RectTransform shadowUI; // El RectTransform de la sombra UI
    public RectTransform indicatorUI; // El RectTransform del indicador de landing UI
    public LayerMask groundLayer; // Capa que representa el suelo
    public float raycastDistance = 20f; // Distancia máxima para detectar el suelo
    public float minScale = 1f; // Escala en el suelo
    public float maxScale = 0.2f; // Escala en el punto más alto
    public float maxJumpHeight = 10f; // Máxima altura esperada

    void FixedUpdate()
    {
        Vector3 playerPosition = rb.position;

        // Lanzamos un rayo hacia abajo para encontrar el suelo
        Ray ray = new Ray(playerPosition, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
        {
            Vector3 groundPos = hit.point;

            // Mover la sombra a la posición horizontal del jugador, pero en el suelo
            shadowUI.position = new Vector3(playerPosition.x, groundPos.y + 0.01f, playerPosition.z);
            indicatorUI.position = new Vector3(playerPosition.x, groundPos.y + 0.01f, playerPosition.z);

            // Calcular altura relativa
            float height = playerPosition.y - groundPos.y;
            height = Mathf.Clamp(height, 0f, maxJumpHeight);

            // Normalizar y escalar
            float t = height / maxJumpHeight;
            float scale = Mathf.Lerp(minScale, maxScale, t);

            shadowUI.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
