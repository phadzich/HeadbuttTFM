using UnityEngine;

public class BlockItemHPPotion : MonoBehaviour
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
