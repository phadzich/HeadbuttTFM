using UnityEngine;

public class CollectibleTrigger : MonoBehaviour
{
    private CollectibleBehaviour parent;

    private void Awake()
    {
        parent = GetComponentInParent<CollectibleBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parent.OnCollected();
        }
    }

}
