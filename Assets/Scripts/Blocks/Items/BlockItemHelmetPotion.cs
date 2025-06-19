using UnityEngine;

public class BlockItemHelmetPotion : MonoBehaviour
{
    public Block parentBlock;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentBlock.Activate();
        }
    }
}
