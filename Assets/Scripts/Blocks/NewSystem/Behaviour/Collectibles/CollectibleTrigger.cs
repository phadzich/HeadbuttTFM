using UnityEngine;

public class CollectibleTrigger : MonoBehaviour
{
    private CollectibleBehaviour parent;

    private void Awake()
    {
        parent = GetComponentInParent<CollectibleBehaviour>();
    }

}
