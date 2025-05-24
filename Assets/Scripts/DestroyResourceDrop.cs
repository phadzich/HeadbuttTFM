using UnityEngine;

public class DestroyResourceDrop : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
