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
        //Debug.DrawRay(origin, direction * 5f, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, 22f))
        {
            //Debug.Log(hit.transform);
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
            //Debug.Log("Move RAW input: " + moveInput);
            // Round each axis to the nearest whole number
            moveInput.x = Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y) ? Mathf.Sign(moveInput.x) : 0;
            moveInput.y = Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) ? Mathf.Sign(moveInput.y) : 0;
            //Debug.Log("Move input: " + moveInput);


            if (!isMoving && !movementLocked)
            {

                var nextPos = positionTarget + new Vector3(moveInput.x, 0, moveInput.y);
                //Debug.Log("NextPos: " + nextPos);
                if (nextPos.x == (-LevelManager.Instance.sublevelWidth - 1) / 2 || nextPos.x > (LevelManager.Instance.sublevelWidth - 1) / 2 || nextPos.z == (-LevelManager.Instance.sublevelHeight - 1) / 2 || nextPos.z > (LevelManager.Instance.sublevelHeight - 1) / 2)
                {
                    Debug.Log("EDGE");
                }
                else
                {
                    ChangePositionTarget(nextPos);
                    isMoving = true;
                }


            }


        }




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
        // Normaliza y convierte a pasos de bloque
        direction = new Vector3(
            Mathf.RoundToInt(Mathf.Sign(direction.x)),
            0,
            Mathf.RoundToInt(Mathf.Sign(direction.z))
        );

        Vector3 newPosition = transform.position + direction;

        // Aquí usas tu sistema de movimiento actual
        // Si tienes un método MoveToPosition(), lo llamas así:
        ChangePositionTarget(newPosition);
    }
}
