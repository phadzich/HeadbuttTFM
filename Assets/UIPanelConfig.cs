using UnityEngine;

public class UIPanelConfig : MonoBehaviour
{
    public bool pauseOnOpen;
    public bool unpauseOnClose;


    private void OnEnable()
    {
        GameManager.Instance.PauseGame(pauseOnOpen);
    }
    private void OnDisable()
    {
        GameManager.Instance.PauseGame(!unpauseOnClose);
    }

    public void OpenGoogleForm() { Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSc1K6Mk7Lu5aLRvEJxpBBAuJGeOzEsi3qPTpSMoP6H5tCkrRw/viewform?usp=dialog"); }
}
