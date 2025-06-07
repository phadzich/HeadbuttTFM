using UnityEngine;

public interface IInteractable
{
    public void EnterZone(GameObject _other);
    public void ExitZone(GameObject _other);
    public void Interact();

    public void ShowInteraction();
}
