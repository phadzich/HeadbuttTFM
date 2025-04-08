using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform cam;

    public float speed = 8.0f;
    public float rotationSpeed = 10.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que el Rigidbody rote por colisiones
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(hor, 0, ver).normalized;

        if (direction.magnitude > 0.1f) // Solo si hay input
        {
            // Rotar el personaje hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Aplicar movimiento
            rb.linearVelocity = direction * speed + new Vector3(0, rb.linearVelocity.y, 0);
        }
        else
        {
            // Detener el personaje cuando no hay input
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
}