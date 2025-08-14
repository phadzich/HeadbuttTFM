using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class WallBehaviour : MonoBehaviour, IBlockBehaviour
{
    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }

    void Start()
    {
        GetComponent<BlockNS>().isWalkable = false;
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }

}
