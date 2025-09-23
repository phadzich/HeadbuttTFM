using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PausePanel : MonoBehaviour
{

    private void OnEnable()
    {
        ChangeInputToUI();
    }

    private void OnDisable()
    {
        if(UIManager.Instance.currentOpenUI != null || UIManager.Instance.dialogueSystem.isRunning) //hay algo abierto previo
        {
            ChangeInputToUI();
        }
        else
        {
            ChangeInputToPlayer();
        }


    }

    private void ChangeInputToUI()
    {
        InputManager.Instance.SwitchInputToUI();
    }

    private void ChangeInputToPlayer()
    {
        InputManager.Instance.SwitchInputToPlayer();
    }


}
