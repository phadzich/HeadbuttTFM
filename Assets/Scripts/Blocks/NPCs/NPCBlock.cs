using Unity.Cinemachine;
using UnityEngine;

public class NPCBlock : Block,IInteractable
{
    public BoxCollider zoneCollider;
    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition = new Vector2(_xPos, _yPos);
        isWalkable = false;
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
    }

    public void ExitZone(GameObject _other)
    {
        Debug.Log("EXITED ZONE");
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }



    public void ShowInteraction()
    {
        throw new System.NotImplementedException();
    }
}
