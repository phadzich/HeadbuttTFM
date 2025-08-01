using UnityEngine;

public class DestroyResourceDrop : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }
    }

}
