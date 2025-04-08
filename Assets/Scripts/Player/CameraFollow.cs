using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al jugador
    public Vector3 offset = new Vector3(0, 3, -5); // Ajusta la posici�n de la c�mara

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target); // La c�mara mira al jugador
        }
    }
}
