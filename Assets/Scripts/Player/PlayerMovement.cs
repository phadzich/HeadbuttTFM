using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float targetYRotation;
    public float rotationSpeed = 10f;

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
    public Transform playerBody;

    [SerializeField]
    float blockLockdownRange;
    public BlockNS blockNSBelow;

    [Header("KNOCKBACK")]
    public float knockbackDistance = 1f;
    public bool isKnockedBack;

    // NUEVO: guardamos el bloque desde el cual empezamos a movernos
    private BlockNS originBlock;

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

        // Cuando llegamos al target, detenemos el movimiento
        if (Vector3.Distance(transform.position, positionTarget) < 0.45f)
        {
            isMoving = false;
        }

        //Si el bloque actual cambió, significa que ya pisamos el nuevo bloque.
        if (originBlock != null && blockNSBelow != null && blockNSBelow != originBlock)
        {
            originBlock = null; // desbloqueamos para permitir nuevo input
        }

        // Rotación suave
        Vector3 currentEuler = playerBody.localEulerAngles;
        float newY = Mathf.LerpAngle(currentEuler.y, targetYRotation, Time.deltaTime * rotationSpeed);
        playerBody.localRotation = Quaternion.Euler(0f, newY, 0f);
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
    }

    void RotatePlayer(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = -angle + 90f;
        targetYRotation = angle;
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

            if (moveInput != Vector2.zero)
            {
                RotatePlayer(moveInput);
            }

            //Bloqueamos input si todavía no pisó el siguiente bloque
            if (originBlock != null)
                return;

            if (!isMoving && movementLocked)
            {
                var nextPos = positionTarget + new Vector3(moveInput.x, 0, moveInput.y);

                if (blockNSBelow != null)
                {
                    if (CanMoveInDirection())
                    {
                        // Guardamos bloque de origen antes de movernos
                        originBlock = blockNSBelow;

                        ChangePositionTarget(nextPos);
                        isMoving = true;

                        if (PlayerManager.Instance.playerStates.currentMainState == PlayerMainStateEnum.Walk)
                        {
                            PlayerManager.Instance.playerAnimations.PlayStepAnimation();
                        }
                    }
                }
            }
        }
    }

    private bool CanMoveInDirection()
    {
        if (blockNSBelow == null) return false;

        if (moveInput.x < 0 && blockNSBelow.left.isWalkable) return true;
        if (moveInput.x > 0 && blockNSBelow.right.isWalkable) return true;
        if (moveInput.y < 0 && blockNSBelow.down.isWalkable) return true;
        if (moveInput.y > 0 && blockNSBelow.up.isWalkable) return true;

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
        positionTarget = new Vector3(0, 50, 0);
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
    }
}
