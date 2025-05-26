using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Componentes")]
    public PlayerStates playerStates;
    public PlayerMovement playerMovement;
    public PlayerCamera playerCamera;
    public PlayerAnimations playerAnimations;
    public PlayerBounce playerBounce;
    //public PlayerHeadbutt playerHeadbutt;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("PlayerManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterMiningLevel()
    {
        playerAnimations.RotateBody(180);
        playerBounce.enabled = true;
    }

    public void EnterNPCLevel()
    {
        playerAnimations.RotateBody(0);
        playerBounce.enabled = false;
    }
}
