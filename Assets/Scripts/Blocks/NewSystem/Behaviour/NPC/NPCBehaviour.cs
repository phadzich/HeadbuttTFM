using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BlockNS))]
public class NPCBehaviour : MonoBehaviour, IInteractable
{
    public BoxCollider zoneCollider;
    public TextMeshProUGUI interactLBL;
    public GameObject interactPanel;
    public string interactString;
    public Sprite icon;
    public Image npcIMG;
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
            npcIMG.sprite = icon;
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
        interactPanel.SetActive(_visibility);
        interactLBL.gameObject.SetActive(_visibility);
        UIManager.Instance.ShowNPCKey(_visibility);
    }
}

public enum NPCType { Door, Crafter, Trader, Elevator, Shop, Inventory }