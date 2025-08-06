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
        SwitchInputToUI();


    }

    private void SwitchInputToUI()
    {
        InputManager.Instance.playerInput.SwitchCurrentActionMap("UI");
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
