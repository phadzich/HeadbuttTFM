using System;
using System.Collections;
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

    private void Start()
    {
        SwitchInputToPlayer();
        Debug.Log($"ActionMap after delay: {InputManager.Instance.playerInput.currentActionMap.name}");
    }

    private void SwitchInputToPlayer()
    {
        StartCoroutine(SwitchDelayed());
    }

    private IEnumerator SwitchDelayed()
    {
        yield return null; // espera 1 frame
        InputManager.Instance.playerInput.SwitchCurrentActionMap("Player");
        Debug.Log($"ActionMap after delay: {InputManager.Instance.playerInput.currentActionMap.name}");
    }

    public void TryInteractWithNPC()
    {
        if (currentInteractableNPC != null)
        {
            currentInteractableNPC.Interact();
        }
    }
}
