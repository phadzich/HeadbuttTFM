using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBounce : MonoBehaviour
{
    Rigidbody rb;
    PlayerStates playerStates;

    [Header("MOVEMENT")]
    [SerializeField]
    float blockLockdownRange;
    [SerializeField]
    Block blockBelow;

    [Header ("BOUNCE")]
    [SerializeField]
    float jumpForce;
    [SerializeField]
    string bounceDirection;

    [Header("HEADBUTT CONFIG")]
    [SerializeField]
    float headbuttRange;
    [SerializeField]
    bool headbuttOnCooldown;
    [SerializeField]
    float headbuttPower;
    [Header("HEADBUTT CHECKS")]
    [SerializeField]
    bool inHeadbuttRange;
    [SerializeField]
    float headbuttCooldown;
    [SerializeField]
    bool timedHeadbutt;
    [SerializeField]
    float timeSinceLastHeadbutt;
    CinemachineImpulseSource impulseSource;
    [SerializeField]
    float maxHeight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStates = GetComponent<PlayerStates>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    void Update()
    {
        //SI ESTA EN ESTADO BOUNE
        CheckForBlockBelow();
        CheckBounceDirection();
        if (bounceDirection == "DOWN")
        {

            CheckForHeadbuttDistance();
            CheckForBounceDistance();
        }
        else
        {
            ToggleHeadbuttIndicator(false);
            playerStates.EnterIdleState();
        }
         
        UpdateHeadbuttCooldown();

        //ESTADO XXX
    }

    private void CheckBounceDirection()
    {
        if(rb.linearVelocity.y >= 0)
        {
            bounceDirection = "UP";
        }
        else
        {
            bounceDirection = "DOWN";
        }
    }

    private void CheckForBlockBelow()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        Debug.DrawRay(origin, direction * 5f, Color.yellow);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, 5f))
        {

            blockBelow = hit.collider.GetComponent<Block>();
        }
    }

    private void CheckMovementLock()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        Debug.DrawRay(origin, direction * blockLockdownRange, Color.yellow);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, blockLockdownRange) && bounceDirection == "DOWN")
        {

            GameManager.Instance.levelMovement.movementLocked = true;
        }
        else
        {
            GameManager.Instance.levelMovement.movementLocked = false;
        }
    }



    private void CheckForBounceDistance()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float _groundDistance = .05f;
        Debug.DrawRay(origin, direction * _groundDistance, Color.red);



        if (Physics.Raycast(origin, direction, out RaycastHit hit, _groundDistance))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponent<Block>())
            {
                BounceUp();
            }

        }
    }

    private void CheckForHeadbuttDistance()
    {

        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        Debug.DrawRay(origin, direction * headbuttRange, Color.red);


        if (Physics.Raycast(origin, direction, out RaycastHit hit, headbuttRange)&&!headbuttOnCooldown)
        {
            //Debug.Log("Headbutt: " + hit.collider.name);
            ToggleHeadbuttIndicator(true);
            playerStates.EnterHeadbuttState();
        }

    }

    private void ToggleHeadbuttIndicator(bool _condition)
    {
        inHeadbuttRange = _condition;

    }

    private void BounceUp()
    {
        //Debug.Log("BOUNCE!");
        rb.linearVelocity = Vector3.zero;
        if (!timedHeadbutt)
        {
            rb.linearVelocity = new Vector3(0, jumpForce, 0);
        }
        blockBelow.Bounce();
        timedHeadbutt = false;
    }

    private void HeadbuttUp()
    {
        Debug.Log("HEADBUTT!");
        rb.linearVelocity = new Vector3(0, headbuttPower, 0);

        blockBelow.Headbutt();
        ScreenShake();
        RestartHeadbuttCooldown();
        GameManager.Instance.IncreaseLevelHBCount(1);
    }

    private void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }

    public void Headbutt(InputAction.CallbackContext context)
    {
        
        //SI EL PLAYER STATE ES GROUNDED
        //EL HEADBUTT SE PUEDE ACTIVAR PARA INTERACTUAR CON EL PISO

        //SI EL STATE ES MINING, ENTONCES SE APLICA LO DE ABAJO

        if(context.phase == InputActionPhase.Performed)
        {
            if (inHeadbuttRange && !headbuttOnCooldown && HelmetManager.Instance.currentHelmet.hasHeadbutts() && blockBelow!=null)
            {
                timedHeadbutt = true;
                Debug.Log("CORRECT!");
                HeadbuttUp();
                timedHeadbutt = false;
            }
            else
            {
                timedHeadbutt = false;
                Debug.Log("MISSED!");
            }
        }

    }

    private void UpdateHeadbuttCooldown()
    {
        timeSinceLastHeadbutt += Time.deltaTime;
        if(timeSinceLastHeadbutt <= headbuttCooldown)
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
