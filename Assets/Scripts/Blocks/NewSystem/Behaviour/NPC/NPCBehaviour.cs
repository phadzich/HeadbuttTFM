using TMPro;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class NPCBehaviour : MonoBehaviour, IInteractable
{
    public BoxCollider zoneCollider;
    public TextMeshProUGUI interactLBL;
    public string interactString;
    public CinemachineCamera NPCCam;

    private int shopID;

    public NPCType type;

    public void SetupBlock(MapContext _context)
    {
        //Debug.Log(this);

        switch (type)
        {
            case NPCType.Shop:
                shopID = _context.npcConfig.npcShopId;
                setWalkable(false);
                break;
            case NPCType.Door:
                setWalkable(true);
                break;
            default:
                setWalkable(false);
                break;
        }

        if (interactLBL != null)
        {
            interactLBL.text = interactString;
        }

    }

    private void setWalkable(bool _walk)
    {
        GetComponent<BlockNS>().isWalkable = _walk;
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
        switch (type)
        {
            case NPCType.Shop:
                Debug.Log("INTERACT");
                UIManager.Instance.OpenShopUI(shopID);
                break;
            default:
                UIManager.Instance.OpenNPCUI(type);
                break;
        }

        UIManager.Instance.ActivateCam(NPCCam);

    }


    public void ShowInteraction(bool _visibility)
    {
        interactLBL.gameObject.SetActive(_visibility);
    }
}

public enum NPCType { Door, Crafter, Trader, Elevator, Shop, Inventory }