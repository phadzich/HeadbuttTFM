using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public PlayerInput playerInput;

    [SerializeField] public NPCBlock currentInteractableNPC;

    private void Awake()
    {

        if(Instance == null)
        {
            Debug.Log("InputManager Awake");
            Instance = this;
        }
        else
        {
            Debug.LogError("M'as de un InputManager");
        }
            Instance = this;
    }

    public void TryInteractWithNPC()
    {
        if (currentInteractableNPC != null)
        {
            currentInteractableNPC.Interact();
        }
    }
}
