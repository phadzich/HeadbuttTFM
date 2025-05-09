using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeadbutt : MonoBehaviour
{
    Rigidbody rb;
    PlayerStates playerStates;
    public GameObject bodyMesh;

    [Header("HEADBUTT CONFIG")]
    [SerializeField]
    float headbuttCooldown;
    [SerializeField]
    bool headbuttOnCooldown;
    [SerializeField]
    float headbuttPower;
    [Header("HEADBUTT CHECKS")]

    [SerializeField]
    float timeSinceLastHeadbutt;
    CinemachineImpulseSource impulseSource;
    [SerializeField]
    float maxHeight;
    [SerializeField]
    string bounceDirection;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        //SI ESTA EN ESTADO BOUNE

        CheckBounceDirection();
        UpdateHeadbuttCooldown();
    }

    private void CheckBounceDirection()
    {
        if (rb.linearVelocity.y >= 0)
        {
            bounceDirection = "UP";
        }
        else
        {
            bounceDirection = "DOWN";
        }
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public void Headbutt(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed)
        {
            if (bounceDirection == "DOWN" && !headbuttOnCooldown &&
                HelmetManager.Instance.currentHelmet.hasHeadbutts() &&
                PlayerManager.Instance.playerMovement.blockBelow != null)
            {
                HeadbuttUp();
            }
        }

    }

    private void HeadbuttUp()
    {
        Debug.Log("HEADBUTT!");
        rb.transform.position = PlayerManager.Instance.playerMovement.blockBelow.transform.position+new Vector3(0,2f,0);
        rb.linearVelocity = new Vector3(0, headbuttPower, 0);

        PlayerManager.Instance.playerMovement.blockBelow.Headbutt();
        ScreenShake();
        RestartHeadbuttCooldown();
        HelmetManager.Instance.currentHelmet.UseHeadbutt();
        PlayerManager.Instance.playerAnimations.HeadbuttSS();
    }

    private void UpdateHeadbuttCooldown()
    {
        timeSinceLastHeadbutt += Time.deltaTime;
        if (timeSinceLastHeadbutt <= headbuttCooldown)
        {
            headbuttOnCooldown = true;
        }
        else
        {
            headbuttOnCooldown = false;
        }

    }

    private void RestartHeadbuttCooldown()
    {
        timeSinceLastHeadbutt = 0;

    }



}
