using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public PlayerInput playerInput;



    [SerializeField] public IInteractable currentInteractableNPC;



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
        SwitchInputToPlayer();
    }
    private void Start()
    {
        StartCoroutine(ForceUIAfterFrames(5));
    }
    public void SwitchInputToUI()
    {
        foreach (var map in playerInput.actions.actionMaps)
            map.Disable();
        InputManager.Instance.playerInput.SwitchCurrentActionMap("UI");
        Debug.Log($"ActionMap after delay: {InputManager.Instance.playerInput.currentActionMap.name}");
    }
    public void SwitchInputToPlayer()
    {
        foreach (var map in playerInput.actions.actionMaps)
            map.Disable();
        InputManager.Instance.playerInput.SwitchCurrentActionMap("Player");
        Debug.Log($"ActionMap after delay: {InputManager.Instance.playerInput.currentActionMap.name}");
    }

    private IEnumerator ForcePlayerAfterFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null; // espera un frame

        SwitchInputToPlayer();
        //Debug.Log("Forzado Player después de " + frames + " frames");
    }

    private IEnumerator ForceUIAfterFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null; // espera un frame

        SwitchInputToUI();
        //Debug.Log("Forzado Player después de " + frames + " frames");
    }

    public void TryInteractWithNPC(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentInteractableNPC != null)
            {
                currentInteractableNPC.Interact();
            }
        }
    }
}
