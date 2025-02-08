using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    [SerializeField]
    float jumpForce;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        BounceUp();
        other.GetComponent<BlockData>().GetHit();
    }

    private void BounceUp()
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up* jumpForce);
    }
}
