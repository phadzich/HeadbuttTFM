using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class DropBehaviour : MonoBehaviour, IBlockBehaviour
{
    public Transform blockMeshParent;
    public bool alreadyBounced;
    
    public void OnBounced(HelmetInstance _helmetInstance)
    {
        //Debug.Log("DROPPED");
        if (alreadyBounced) return;

        alreadyBounced = true;

        PlayerStates playerStates = PlayerManager.Instance.playerStates;
        if (playerStates.onMiningLvl)
        {
            //Debug.Log("ON MINING LEVEL STATE");
            playerStates.ChangeState(PlayerMainStateEnum.Bouncing);
        } else
        {
            //Debug.Log("ON NPC LEVEL STATE");
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
