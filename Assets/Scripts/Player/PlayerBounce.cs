using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static UnityEngine.UI.Image;

public class PlayerBounce : MonoBehaviour
{
    Rigidbody rb;
    PlayerStates playerStates;
    [SerializeField] private LayerMask blockLayerMask;
    public GameObject bodyMesh;

    [Header ("BOUNCE")]
    [SerializeField]
    float jumpForce;
    [SerializeField]
    public string bounceDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStates = GetComponent<PlayerStates>();
    }


    void Update()
    {

        if (!PlayerManager.Instance.playerStates.canBounce)
        {
            return; // no puede saltar
        }

        CheckBounceDirection();
        if (bounceDirection == "DOWN")
        {

            CheckForBounceDistance();
        }
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
        PlayerManager.Instance.playerMovement.bounceDirection = bounceDirection;
    }


    private void CheckForBounceDistance()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float _groundDistance = .5f;


        if (Physics.Raycast(origin, direction, out RaycastHit hit, _groundDistance, blockLayerMask))
        {
            if (hit.collider.gameObject.GetComponent<BlockNS>())
            {
                BounceUp();
            }

        }
    }

    public void BounceUp()
    {
        PlayerManager.Instance.playerStates.ChangeState(PlayerMainStateEnum.Bouncing);

        jumpForce = 5;
        rb.linearVelocity = Vector3.zero;
        rb.linearVelocity = new Vector3(0, jumpForce, 0);
        //PlayerManager.Instance.playerMovement.blockBelow.OnBounced(HelmetManager.Instance.currentHelmet);
        PlayerManager.Instance.playerMovement.blockNSBelow.OnBounced(HelmetManager.Instance.currentHelmet);
        PlayerManager.Instance.playerAnimations.BounceSS();
        PlayerManager.Instance.playerAnimations.BounceSS();
        HelmetManager.Instance.currentHelmet.OnBounce();
    }

    }
