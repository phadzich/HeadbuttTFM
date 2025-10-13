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
    public bool isRunning;
    public DialoguePanelUI dialogueUI;
    public List<GameObject> obHighlights;

    public void StartDialogue(DialogueSequence _sequence)
    {
        //Debug.Log("DIALOG START");
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
        isRunning = true;
}

    public void NextLinePressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

            ShowNextLine();

        }
    }

    private IEnumerator SendDialogueSFX(AudioClip _clip, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SoundManager.PlayDialog(_clip);
    }
    public void ShowNextLine()
    {
            if (index < lines.Count)
            {
                if (index != 0)
                {
                    StartCoroutine(SendDialogueSFX(lines[index - 1].jomaSFX, 0));
                }

            StartCoroutine(SendDialogueSFX(lines[index].npcSFX, lines[index].jomaSFX.length));
            TryShowHighlight(lines[index].highlightID);
            dialogueUI.UpdateDialogContent(lines[index]);
                index++;


        }
            else
            {
            StartCoroutine(SendDialogueSFX(lines[index-1].jomaSFX, 0));
            EndDialogue();

        }
    }

    private void TryShowHighlight(int _index)
    {
        HideAllHighlights();

        if (_index == 0)
        {
            return;
        }
        else
        {
            var _highOBJ = UIManager.Instance.dialogueSystem.obHighlights[_index];
            _highOBJ.SetActive(true);
        }
    }

    private void HideAllHighlights()
    {
        foreach (GameObject _highlight in UIManager.Instance.dialogueSystem.obHighlights)
        {
            if (_highlight != null) _highlight.SetActive(false);
        }
    }



    void EndDialogue()
    {
        dialogueUI.Close();
        SwitchInputToPlayer();
        HideAllHighlights();
        isRunning = false;
    }

}
