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
        Debug.Log("DIALOG START");
        lines = _sequence.lines;
        index = 0;
        ShowDialogueUI();
        ShowNextLine();
        StartCoroutine(ForceUIAfterFrames(10));
    }

    private void SwitchInputToUI()
    {
        InputManager.Instance.SwitchInputToUI();
    }

    private IEnumerator ForceUIAfterFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null; // espera un frame

        SwitchInputToUI();
        Debug.Log("Forzado UI después de " + frames + " frames");
    }

    private void SwitchInputToPlayer()
    {

        InputManager.Instance.SwitchInputToPlayer();

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
