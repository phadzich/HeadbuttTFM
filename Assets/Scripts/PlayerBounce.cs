using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBounce : MonoBehaviour
{
    [SerializeField]
    float jumpForce;
    Rigidbody rb;
    [SerializeField]
    bool inHeadbuttRange;
    [SerializeField]
    float headbuttRange;
    [SerializeField]
    float headbuttPower;
    [SerializeField]
    float timeSinceLastHeadbutt;
    [SerializeField]
    float headbuttCooldown;
    [SerializeField]
    bool headbuttOnCooldown;

    public PlayerStates playerStates;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStates = GetComponent<PlayerStates>();
    }


    void Update()
    {
        CheckForHeadbuttDistance();
        UpdateHeadbuttCooldown();
    }

    private void CheckForHeadbuttDistance()
    {
        // Ray start position
        Vector3 origin = transform.position;

        // Direction downwards
        Vector3 direction = Vector3.down;


        // Draw a green ray in the Scene view
        Debug.DrawRay(origin, direction * headbuttRange, Color.red);

        // Perform the raycast
        if (Physics.Raycast(origin, direction, out RaycastHit hit, headbuttRange)&&!headbuttOnCooldown)
        {
            //Debug.Log("Headbutt: " + hit.collider.name);
            ToggleHeadbuttIndicator(true);
            playerStates.EnterHeadbuttState();
        }
        else
        {
            ToggleHeadbuttIndicator(false);
            playerStates.EnterIdleState();
        }
    }

    private void ToggleHeadbuttIndicator(bool _condition)
    {
        inHeadbuttRange = _condition;
        GameManager.instance.levelMovement.movementLocked = _condition;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        BounceUp();
        other.GetComponent<BlockData>().GetHit();
    }

    private void BounceUp()
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up* jumpForce);
    }

    public void Headbutt(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if (inHeadbuttRange && !headbuttOnCooldown)
            {
                Debug.Log("HEADBUTT!");
                rb.linearVelocity = Vector3.zero;
                rb.AddForce(Vector3.up * headbuttPower);
                RestartHeadbuttCooldown();
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
