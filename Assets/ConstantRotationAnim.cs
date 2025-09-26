using UnityEngine;

public class ConstantRotationAnim : MonoBehaviour
{

    // grados por segundo en cada eje
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f);

    void Update()
    {
        // rotar en local space, multiplicando por deltaTime para hacerlo frame-rate independent
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}