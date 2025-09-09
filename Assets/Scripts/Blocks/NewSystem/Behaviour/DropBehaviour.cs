using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class DropBehaviour : MonoBehaviour, IBlockBehaviour
{
    public Transform blockMeshParent;
    
    public void OnBounced(HelmetInstance _helmetInstance)
    {
        Debug.Log("DROPPED");
        PlayerStates playerStates = PlayerManager.Instance.playerStates;
        if (playerStates.onMiningLvl)
        {
            playerStates.ChangeState(PlayerMainStateEnum.Bouncing);
        } else
        {
            playerStates.ChangeState(PlayerMainStateEnum.Walk);
        }
        
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
