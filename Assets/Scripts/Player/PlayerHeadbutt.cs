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



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {

        UpdateHeadbuttCooldown();
        KeepCentered();
    }

    private void KeepCentered()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
    }


    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public void Headbutt(InputAction.CallbackContext context)
    {
        // por ahora quite bounceDirection == "DOWN" && 
        if (context.phase == InputActionPhase.Performed)
        {
            if (!headbuttOnCooldown &&
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
