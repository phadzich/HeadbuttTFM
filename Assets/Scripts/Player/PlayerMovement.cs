using System.Collections;
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
    public Vector3 restartPosition;
    public float speed = 10f;
    public float normalSpeed = 10f;
    public float dropSpeed = 2f;
    [SerializeField]
    private bool isMoving;
    public bool movementLocked;
    public string bounceDirection;

    [Header("DISTANCE TO BLOCK BELOW")]
    [SerializeField] private LayerMask blockLayerMask;
    public Transform enanoParent;
    [SerializeField]
    float blockLockdownRange;
    public BlockNS blockNSBelow;

    [Header("KNOCKBACK")]
    public float knockbackDistance = 1f;
    public bool isKnockedBack;

    private void Start()
    {
        positionTarget = transform.position;
        restartPosition = positionTarget;
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

            blockNSBelow = hit.collider.GetComponent<BlockNS>();
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
        if (!PlayerManager.Instance.playerStates.canMove) return;
        if (isKnockedBack) return;

        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
            moveInput.x = Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y) ? Mathf.Sign(moveInput.x) : 0;
            moveInput.y = Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) ? Mathf.Sign(moveInput.y) : 0;

            if (!isMoving && movementLocked)
            {

                var nextPos = positionTarget + new Vector3(moveInput.x, 0, moveInput.y);

                if (blockNSBelow != null)
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
            if (blockNSBelow.left.isWalkable)
            {
                return true;
            }
        }
        else if (moveInput.x > 0)
        {
            if (blockNSBelow.right.isWalkable)
            {
                return true;
            }
        }
        else if (moveInput.y < 0)
        {
            if (blockNSBelow.down.isWalkable)
            {
                return true;
            }
        }
        else if (moveInput.y > 0)
        {
            if (blockNSBelow.up.isWalkable)
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

        bool blockBelow = Physics.Raycast(origin, direction, out RaycastHit hit, blockLockdownRange) && bounceDirection == "DOWN";

        movementLocked = !blockBelow;
    }

    public void Knockback(Vector3 direction)
    {
        isKnockedBack = true;

        Vector3 alignedPosition = new Vector3(
            blockNSBelow.transform.position.x,
            0,
            blockNSBelow.transform.position.z
        );

        Vector3 newPosition = alignedPosition + direction;


        ChangePositionTarget(newPosition);

        StartCoroutine(ReleaseKnockbackLock());
    }
    private IEnumerator ReleaseKnockbackLock()
    {
        yield return new WaitForSeconds(0.20f);
        isKnockedBack = false;
    }

    public void RespawnPlayer()
    {
        positionTarget = new Vector3(0,50,0);
        enanoParent.position = positionTarget;
        ChangePositionTarget(positionTarget);
    }

    public void MoveToDrop(Vector3 _dropPosition)
    {
        StartCoroutine(DelayMoveToDrop(_dropPosition));
    }

    public IEnumerator RestoreToNormalSpeed()
    {
        yield return new WaitForSeconds(1.5f);
        speed = normalSpeed;
    }

    public IEnumerator DelayMoveToDrop(Vector3 _dropPosition)
    {
        yield return new WaitForSeconds(1f);
        speed = dropSpeed;
        var _newPos = new Vector3(_dropPosition.x, _dropPosition.y + 50f, _dropPosition.z);
        positionTarget = _newPos;
        ChangePositionTarget(_newPos);
        StartCoroutine(RestoreToNormalSpeed());
        //Debug.Log($"Player falling at {_newPos}");
    }


}
