using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI currentHelmetTitle;
    public Image currentHelmetImg;
    public TextMeshProUGUI currentBounces;
    public TextMeshProUGUI currentHB;
    public ResourceIndicator bounceResource;
    public ResourceIndicator HBresource;
    public Button buyBounceBtn;
    public Button buyHBbtn;

    [Header("Prices")]
    public int jumpQuantity = 3;
    public int headbuttQuantity = 2;

    private int currentHelmetIndex = 0;
    private HelmetInstance currentHelmet;
    public int helmetCount;
    private ResourceRequirement bounceRes;
    private ResourceRequirement HBres;

    private void Awake()
    {
        Debug.Log("CRAFT");

        bounceRes = CraftingManager.Instance.bouncePrice;
        HBres = CraftingManager.Instance.headbuttPrice;

        helmetCount = HelmetManager.Instance.helmetsEquipped.Count;
        currentHelmet = HelmetManager.Instance.helmetsEquipped[currentHelmetIndex];

        updateUI();
        HasEnoughBounceResources();
        HasEnoughHbResources();
        UpdateBounceResource();
        UpdateHbResource();
    }

    public void BuyBounce()
    {
        if(ResourceManager.Instance.SpendResource(bounceRes.resource, bounceRes.quantity))
        {
            currentHelmet.upgradeJump(jumpQuantity);
            UpdateBounceResource();
            UpdateBouncesTxt();
        }

        HasEnoughBounceResources();

    }

    public void BuyHeadbutt()
    {
        if (ResourceManager.Instance.SpendResource(HBres.resource, HBres.quantity))
        {
            currentHelmet.upgradeHeadbutt(headbuttQuantity);
            UpdateHbResource();
            UpdateHbTxt();
        }

        HasEnoughHbResources();

    }

    public void PrevHelmet()
    {
        PreviousIndex();
        currentHelmet = HelmetManager.Instance.helmetsEquipped[currentHelmetIndex];
        updateUI();
    }

    public void NextHelmet()
    {
        NextIndex();
        currentHelmet = HelmetManager.Instance.helmetsEquipped[currentHelmetIndex];
        updateUI();
    }

    private void NextIndex()
    {
        currentHelmetIndex = (currentHelmetIndex + 1) % helmetCount;
    }

    private void PreviousIndex()
    {
        currentHelmetIndex = (currentHelmetIndex - 1 + helmetCount) % helmetCount;
    }

    private void HasEnoughBounceResources()
    {
        if(ResourceManager.Instance.GetOwnedResourceAmount(bounceRes.resource) >= bounceRes.quantity)
        {
            EnableBounceButton(true);
        }
        else
        {
            EnableBounceButton(false);
        }
    }
    private void HasEnoughHbResources()
    {
        if (ResourceManager.Instance.GetOwnedResourceAmount(HBres.resource) >= HBres.quantity)
        {
            EnableHbButton(true);
        }
        else
        {
            EnableHbButton(false);
        }
    }

    // Update UI Functions

    //Activate Panel
    public void ActivatePanel()
    {
        gameObject.SetActive(true);
        currentHelmetIndex = 0;
        updateUI();
        UpdateBounceResource();
        UpdateHbResource();
        HasEnoughBounceResources();
        HasEnoughHbResources();
    }

    // Actualiza la información del casco
    private void updateUI()
    {
        currentHelmetTitle.text = currentHelmet.baseHelmet.helmetName;
        currentHelmetImg.sprite = currentHelmet.baseHelmet.icon;
        UpdateBouncesTxt();
        UpdateHbTxt();
    }

    private void UpdateBounceResource()
    {
        bounceResource.SetupIndicator(bounceRes.resource, ResourceManager.Instance.GetOwnedResourceAmount(bounceRes.resource));
    }

    private void UpdateHbResource()
    {
        HBresource.SetupIndicator(HBres.resource, ResourceManager.Instance.GetOwnedResourceAmount(HBres.resource));
    }

    // Actualiza la cantidad de saltos del casco actual en el UI
    private void UpdateBouncesTxt()
    {
        currentBounces.text = "Saltos: " + currentHelmet.bounces.ToString();
    }

    // Actualiza la cantidad de headbutts del casco actual en el UI
    private void UpdateHbTxt()
    {
        currentHB.text = "Headbutts: " + currentHelmet.maxHeadbutts.ToString();
    }

    private void EnableBounceButton(bool enable)
    {
        buyBounceBtn.interactable = enable;
    }

    private void EnableHbButton(bool enable)
    {
        buyHBbtn.interactable = enable;
    }
}
