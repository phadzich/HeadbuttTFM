using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class DropBehaviour : MonoBehaviour, IBlockBehaviour
{
    public Transform blockMeshParent;
    
    public void OnBounced(HelmetInstance _helmetInstance)
    {
        Debug.Log("DROPPED");
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {

    }

    public void Activate()
    {
         
    }

    public void StartBehaviour()
    {
        LevelManager.Instance.currentDropBlock = gameObject;
    }

    public void StopBehaviour()
    {

    }
}
