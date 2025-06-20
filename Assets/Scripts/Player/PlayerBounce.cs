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
    public GameObject bodyMesh;

    [Header ("BOUNCE")]
    [SerializeField]
    float jumpForce;
    [SerializeField]
    string bounceDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStates = GetComponent<PlayerStates>();
    }


    void Update()
    {
        //SI ESTA EN ESTADO BOUNE

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
            if (bounceDirection == "DOWN")
            {

            }
            bounceDirection = "UP";

        }
        else
        {
            if (bounceDirection == "UP")
            {
            }
            bounceDirection = "DOWN";
        }
        PlayerManager.Instance.playerMovement.bounceDirection = bounceDirection;
    }


    private void CheckForBounceDistance()
    {
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float _groundDistance = .5f;
        Debug.DrawRay(origin, direction * _groundDistance, Color.red);


        if (Physics.Raycast(origin, direction, out RaycastHit hit, _groundDistance))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponent<Block>())
            {
                //Debug.Log("HITBLOCK");
                BounceUp();
            }

        }
    }

    private void BounceUp()
    {
        jumpForce = HelmetManager.Instance.currentHelmet.bounceHeight;
        //Debug.Log("BOUNCE!");
        rb.linearVelocity = Vector3.zero;
            rb.linearVelocity = new Vector3(0, jumpForce, 0);
            PlayerManager.Instance.playerMovement.blockBelow.Bounce();
        PlayerManager.Instance.playerAnimations.BounceSS();
    }

    }
