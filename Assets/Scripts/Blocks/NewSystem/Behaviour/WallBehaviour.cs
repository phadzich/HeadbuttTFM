using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class WallBehaviour : MonoBehaviour
{

    void Start()
    {
        GetComponent<BlockNS>().isWalkable = false;
    }
    
}
