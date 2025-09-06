using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public PlayerInput playerInput;



    [SerializeField] public IInteractable currentInteractableNPC;

    private void Update()
    {
        //Debug.Log(InputManager.Instance.playerInput.currentActionMap.name);
        //Debug.Log(playerInput.actions.FindAction("Move").enabled);
    }

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
