using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class NPCBlock : Block,IInteractable
{
    public BoxCollider zoneCollider;
    public TextMeshProUGUI interactLBL;
    public string interactString;

    public NPCType type;
    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        //Debug.Log(this);
        sublevelId = _subId;
        sublevelPosition = new Vector2(_xPos, _yPos);
        isWalkable = false;
        if (interactLBL != null)
        {
            interactLBL.text = interactString;
        }
 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterZone(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitZone(other.gameObject);
        }

    }

    public void EnterZone(GameObject _other)
    {
        Debug.Log("ENTERED ZONE");
        InputManager.Instance.currentInteractableNPC = this;
        ShowInteraction(true);
    }

    public void ExitZone(GameObject _other)
    {
        Debug.Log("EXITED ZONE");
        InputManager.Instance.currentInteractableNPC = null;
        ShowInteraction(false);
    }

    public void Interact()
    {
        UIManager.Instance.OpenNPCUI(type);
    }


    public void ShowInteraction(bool _visibility)
    {
        interactLBL.gameObject.SetActive(_visibility);
    }

}

public enum NPCType { Crafter, Upgrader, Elevator }
