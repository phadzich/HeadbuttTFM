using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    private List<DialogueLine> lines;
    private int index = 0;

    public DialoguePanelUI dialogueUI;


    public void StartDialogue(DialogueSequence _sequence)
    {
        lines = _sequence.lines;
        index = 0;
        ShowDialogueUI();
        ShowNextLine();
        SwitchInputToPlayer();
        SwitchInputToUI();
    }

    private void SwitchInputToUI()
    {
        StartCoroutine(SwitchDelayed());
    }

    private IEnumerator SwitchDelayed()
    {
        yield return null; // espera 1 frame
        InputManager.Instance.playerInput.SwitchCurrentActionMap("UI");
        Debug.Log($"ActionMap after delay: {InputManager.Instance.playerInput.currentActionMap.name}");
    }

    private void SwitchInputToPlayer()
    {

        InputManager.Instance.playerInput.SwitchCurrentActionMap("Player");

    }
    private void ShowDialogueUI()
    {
        dialogueUI.Open();
    }

    public void NextLinePressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ShowNextLine();
        }
    }

    public void ShowNextLine()
    {
            if (index < lines.Count)
            {
                dialogueUI.UpdateDialogContent(lines[index]);
                index++;
            }
            else
            {
                EndDialogue();
        }
    }



    void EndDialogue()
    {
        dialogueUI.Close();
        SwitchInputToPlayer();
    }

}
