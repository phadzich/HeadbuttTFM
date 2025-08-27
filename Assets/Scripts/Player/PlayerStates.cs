using System.Collections;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    public LevelManager level;
    public Material idleMaterial;
    public Material headbuttMaterial;
    public MeshRenderer bodyMeshRenderer;
    public PlayerMovement playerMovement;

    public bool isStunned;


    public void GetStunned(float _secondsStunned)
    {
        if (isStunned) return;
        StartCoroutine(StartCooldown(_secondsStunned));
    }

    public void InterruptEffect()
    {
        Debug.Log("PLAYER IS NOT STUNNED");
        isStunned = false;
        StopAllCoroutines();
    }

    private IEnumerator StartCooldown(float _secondsStunned)
    {
        Debug.Log("PLAYER IS STUNNED");
        isStunned = true;
        PlayerManager.Instance.DeactivateMoving();
        yield return new WaitForSeconds(_secondsStunned);
        Debug.Log("PLAYER IS NOT STUNNED");
        isStunned = false;
        PlayerManager.Instance.ActivateMoving();
        PlayerManager.Instance.playerBounce.BounceUp();
    }


    public void EnterIdleState()
    {
        bodyMeshRenderer.material = idleMaterial;
    }
    public void EnterHeadbuttState()
    {
        bodyMeshRenderer.material = headbuttMaterial;
    }

}
