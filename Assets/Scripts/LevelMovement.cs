using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 moveInput;
    private Vector3 positionTarget;
    public float speed = 10f;
    private bool isMoving;
    public bool movementLocked;


    private void Start()
    {
        positionTarget = transform.position;
    }
    private void Update()
    {

            transform.position = Vector3.Lerp(transform.position, positionTarget, Time.deltaTime * speed);


        if (Vector3.Distance(transform.position, positionTarget) < 0.5f)
        {

            isMoving = false; // Stop moving when close
        }
    }

    private void ChangePositionTarget(Vector3 newPos)
    {
        positionTarget = new Vector3(newPos.x,0,newPos.z);
        //Debug.Log("NewTarget: " + positionTarget);
         
    }

    public void MoveAllBlocks(InputAction.CallbackContext context)
    {


        if (context.phase == InputActionPhase.Performed){
            moveInput = context.ReadValue<Vector2>();
            //Debug.Log("Move RAW input: " + moveInput);
            // Round each axis to the nearest whole number
            moveInput.x = Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y) ? Mathf.Sign(moveInput.x) : 0;
            moveInput.y = Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x) ? Mathf.Sign(moveInput.y) : 0;
            //Debug.Log("Move input: " + moveInput);


            if (!isMoving && !movementLocked)
            {

                var nextPos = positionTarget + new Vector3(moveInput.x * -1, 0, moveInput.y * -1);
                //Debug.Log("NextPos: " + nextPos);
                if(nextPos.x == (-LevelManager.Instance.sublevelWidth-1)/2 || nextPos.x > (LevelManager.Instance.sublevelWidth - 1) / 2 || nextPos.z == (-LevelManager.Instance.sublevelHeight - 1) / 2 || nextPos.z > (LevelManager.Instance.sublevelHeight - 1) / 2) {
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


}
