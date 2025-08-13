using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanelConfig : MonoBehaviour
{
    public bool pauseOnOpen;
    public bool unpauseOnClose;


    private void OnEnable()
    {
        InputManager.Instance.playerInput.SwitchCurrentActionMap("UI");
        //GameManager.Instance.PauseGame(pauseOnOpen);
    }
    private void OnDisable()
    {
        InputManager.Instance.playerInput.SwitchCurrentActionMap("Player");
        UIManager.Instance.DeactivateCurrentCam();
        //GameManager.Instance.PauseGame(!unpauseOnClose);
    }

    public void OpenGoogleForm() { Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSc1K6Mk7Lu5aLRvEJxpBBAuJGeOzEsi3qPTpSMoP6H5tCkrRw/viewform?usp=dialog"); }
}
