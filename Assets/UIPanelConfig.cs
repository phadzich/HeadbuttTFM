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
}
