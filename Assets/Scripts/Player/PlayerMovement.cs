using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("INPUT")]
    [SerializeField]
    private Vector2 moveInput;

    [Header("MOVEMENT")]
    [SerializeField]
    public Vector3 positionTarget;
    public float speed = 10f;
    [SerializeField]
    private bool isMoving;
    public bool movementLocked;
    public string bounceDirection;

    [Header("DISTANCE TO BLOCK BELOW")]
    [SerializeField] private LayerMask blockLayerMask;
    public Transform enanoParent;
    [SerializeField]
    float blockLockdownRange;
    public Block blockBelow;

    [Header("KNOCKBACK")]
    public float knockbackDistance = 1f;

    private void Start()
    {
        positionTarget = transform.position;
    }
    private void Update()
    {

        CheckForBlockBelow();
        CheckMovementLock();

        transform.position = Vector3.Lerp(transform.position, positionTarget, Time.deltaTime * speed);
        //Debug.Log(transform.position);

        if (Vector3.Distance(transform.position, positionTarget) < 0.5f)
        {

            isMoving = false; // Stop moving when close
        }
    }

    private void CheckForBlockBelow()
    {
        Vector3 origin = enanoParent.transform.position;
        Vector3 direction = Vector3.down;
        Debug.DrawRay(origin, direction * 20f, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, 20f, blockLayerMask))
        {
            
            blockBelow = hit.collider.GetComponent<Block>();
        }
    }

    public void ChangePositionTarget(Vector3 newPos)
    {
        Vector3 alignedPosition = new Vector3(
    Mathf.Round(newPos.x),
    Mathf.Round(newPos.y),
    Mathf.Round(newPos.z)
);
        positionTarget = new Vector3(alignedPosition.x, 0, alignedPosition.z);
        //Debug.Log("NewTarget: " + positionTarget);

    }



    public void MovePlayer(InputAction.CallbackContext context)
    {


        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
            moveInput.x = Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y) ? Mathf.Sign(moveInput.x) : 0;
            moveInput.y = Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) ? Mathf.Sign(moveInput.y) : 0;

            if (!isMoving && !movementLocked)
            {

                var nextPos = positionTarget + new Vector3(moveInput.x, 0, moveInput.y);

                if (blockBelow != null)
                {
                    {
                        if (CanMoveInDirection())
                        {
                            ChangePositionTarget(nextPos);
                            isMoving = true;
                        }
                    }
                }


            }
        }
    }

    private bool CanMoveInDirection()
    {
        if (moveInput.x < 0)
        {
            if (blockBelow.left.isWalkable)
            {
                return true;
            }
        }
        else if (moveInput.x > 0)
        {
            if (blockBelow.right.isWalkable)
            {
                return true;
            }
        }
        else if (moveInput.y < 0)
        {
            if (blockBelow.down.isWalkable)
            {
                return true;
            }
        }
        else if (moveInput.y > 0)
        {
            if (blockBelow.up.isWalkable)
            {
                return true;
            }
        }
        return false;
    }

    private void CheckMovementLock()
    {
        Vector3 origin = enanoParent.transform.position;
        Vector3 direction = Vector3.down;
        //Debug.DrawRay(origin, direction * blockLockdownRange, Color.yellow);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, blockLockdownRange) && bounceDirection == "DOWN")
        {

            GameManager.Instance.playerMovement.movementLocked = true;
        }
        else
        {
            GameManager.Instance.playerMovement.movementLocked = false;
        }
    }

    public void Knockback(Vector3 direction)
    {
        if (KnockbackChance())
        {
            Vector3 alignedPosition = new Vector3(
    Mathf.Round(transform.position.x),
    0,
    Mathf.Round(transform.position.z)
);
            Vector3 newPosition = alignedPosition + direction;
            //Debug.Log($"Knockback position: {newPosition}");
            ChangePositionTarget(newPosition);
        }

    }

    public bool KnockbackChance()
    {
        bool _shouldKnock = true;
        int _chance = 100;
        int _random = Random.Range(0, 100);
        //Debug.Log(_chance);
        //Debug.Log(_random);
        if (_random <= _chance)
        {
            _shouldKnock = true;
        }
        else
        {
            _shouldKnock = false;
        }
        //Debug.Log(_shouldKnock);
        return _shouldKnock;
    }
}
