using PrimeTween;
using Unity.Cinemachine;

using UnityEngine;


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
    private bool justBounced;
    private bool bounceLocked = false;

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

            CheckForBounceDistance();
    }


    private void CheckForBounceDistance()
    {
        if (bounceLocked) return; //bloquea hasta el próximo FixedUpdate

        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float _groundDistance = .5f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, _groundDistance, blockLayerMask))
        {
            if (!justBounced && hit.collider.gameObject.GetComponent<BlockNS>()&& PlayerManager.Instance.playerStates.currentMainState!= PlayerMainStateEnum.Headbutt)
            {
                BounceUp();
                bounceLocked = true; //evita múltiples en un frame
            }
        }
        else
        {
            justBounced = false;
        }
    }

    public void BounceUp()
    {
        PlayerManager.Instance.playerStates.ChangeState(PlayerMainStateEnum.Bouncing);
        //Debug.Log("BOUNCE");

        jumpForce = 5;
        rb.linearVelocity = Vector3.zero;
        rb.linearVelocity = new Vector3(0, jumpForce, 0);

        PlayerManager.Instance.playerMovement.blockNSBelow.OnBounced(HelmetManager.Instance.currentHelmet);
        PlayerManager.Instance.playerAnimations.BounceSS();
        HelmetManager.Instance.currentHelmet.OnBounce();

        justBounced = true;
    }

    private void FixedUpdate()
    {
        bounceLocked = false; // se resetea en el ciclo de física
    }

}
