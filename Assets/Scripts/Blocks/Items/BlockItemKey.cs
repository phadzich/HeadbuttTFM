using UnityEngine;

public class BlockItemKey : MonoBehaviour
{
    public Block _parentBlock;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _parentBlock.Activate();
        }
    }
}
