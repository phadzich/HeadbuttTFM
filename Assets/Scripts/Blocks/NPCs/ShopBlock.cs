using TMPro;
using UnityEngine;

public class ShopBlock : Block,IInteractable
{
    public BoxCollider zoneCollider;
    public TextMeshProUGUI interactLBL;
    public string interactString;

    public int shopID;
    public void SetupBlock(int _subId, int _xPos, int _yPos, int _shopID)
    {
        //Debug.Log(this);
        sublevelId = _subId;
        shopID = _shopID;
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
        //Debug.Log("ENTERED ZONE");
        InputManager.Instance.currentInteractableNPC = this;
        ShowInteraction(true);
    }

    public void ExitZone(GameObject _other)
    {
        //Debug.Log("EXITED ZONE");
        InputManager.Instance.currentInteractableNPC = null;
        ShowInteraction(false);
    }

    public void Interact()
    {
        UIManager.Instance.OpenShopUI(shopID);
    }


    public void ShowInteraction(bool _visibility)
    {
        interactLBL.gameObject.SetActive(_visibility);
    }

}