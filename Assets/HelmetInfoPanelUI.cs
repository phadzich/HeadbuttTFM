using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetInfoPanelUI : MonoBehaviour
{
    private HelmetInstance helmetInstance;
    public string nextAction;

    public TextMeshProUGUI rarityTXT;
    public TextMeshProUGUI nameTXT;
    public TextMeshProUGUI loreTXT;
    public TextMeshProUGUI lvlTXT;
    public TextMeshProUGUI effTypeTXT;
    public TextMeshProUGUI effNameTXT;
    public TextMeshProUGUI effDescTXT;
    public TextMeshProUGUI effCostTXT;
    public TextMeshProUGUI hpTXT;
    public TextMeshProUGUI powerTXT;
    public TextMeshProUGUI strongTXT;
    public TextMeshProUGUI res01TXT;
    public TextMeshProUGUI res02TXT;
    public TextMeshProUGUI equipBtnTXT;

    public Image helmetIcon;
    public Image elementIcon;
    public Image elementPanel;
    public Image effectIcon;
    public Image effectIconPanel;
    public Image res01Icon;
    public Image res02Icon;

    public GameObject equippedLabel;
    public Button upgradeBtn;
    public Button equipBtn;


    public List<Sprite> elementIcons;
    public List<Color> elementColors;

    public void UpdateInfoCard(HelmetInstance _helmetInstance)
    {
        this.gameObject.SetActive(true);
        helmetInstance = _helmetInstance;
        UpdateData();
        UpdateUpgradeButton();

    }

    public void UpdateData()
    {
        rarityTXT.text = helmetInstance.baseHelmet.rarity.ToString();
        nameTXT.text = helmetInstance.baseHelmet.helmetName;
        ColorHelmetNameGradient();

        loreTXT.text = helmetInstance.baseHelmet.description;
        lvlTXT.text = $"LVL.{helmetInstance.currentLevel}";

        effTypeTXT.text = helmetInstance.baseHelmet.effects[0].effectType;
        effNameTXT.text = helmetInstance.baseHelmet.effects[0].effectName;
        effCostTXT.text = (helmetInstance.baseHelmet.effects[0].hbPointsUsed + 1).ToString();

        UpdateLevelData();
        powerTXT.text = ((int)helmetInstance.baseHelmet.miningPower + 1).ToString();

        helmetIcon.sprite = helmetInstance.baseHelmet.icon;


        elementIcon.sprite = elementIcons[(int)helmetInstance.baseHelmet.element];
        elementPanel.color = elementColors[(int)helmetInstance.baseHelmet.element];

        effectIcon.sprite = helmetInstance.baseHelmet.effects[0].effectIcon;
        effectIconPanel.color = elementColors[(int)helmetInstance.baseHelmet.element];

        equippedLabel.SetActive(helmetInstance.isEquipped);

    }

    private void UpdateUpgradeButton()
    {

        //Debug.Log(helmetInstance.currentLevel);

        //MAXED
        if(helmetInstance.currentLevel == 3)
        {
            nextAction = "MAXED!";
            EnableUpgrade(false);
            EnableEquip(true);
            upgradeBtn.gameObject.SetActive(false);
            return;
        }

        UpgradeRequirement _requirements = helmetInstance.baseHelmet.levelUpRequirements[helmetInstance.currentLevel];

        //discovered
        if (helmetInstance.isDiscovered)
        {
            nextAction = "CRAFT!";
            EnableEquip(false);
            if (CraftingManager.Instance.CanCraft(helmetInstance.GetUpgradeRequirement()))
            {
                EnableUpgrade(true);

            }
            else
            {
                EnableUpgrade(false);
            }

        }

        //crafted
        if (helmetInstance.currentLevel == 1 || helmetInstance.currentLevel == 2)
        {
            nextAction = "LEVEL UP!";
            EnableEquip(true);
            if (CraftingManager.Instance.CanCraft(helmetInstance.GetUpgradeRequirement()))
            {
                EnableUpgrade(true);
            }
            else
            {
                EnableUpgrade(false);
            }
        }


        res01Icon.sprite = _requirements.requirements[0].resource.icon;
        res01TXT.text = _requirements.requirements[0].quantity.ToString();

        if (_requirements.requirements.Count > 1)
        {
            res02Icon.sprite = _requirements.requirements[1].resource.icon;
            res02TXT.text = _requirements.requirements[1].quantity.ToString();
        }
        else
        {
            res02Icon.sprite = null;
            res02TXT.text = "";
        }

        if (helmetInstance.isEquipped)
        {
            EnableEquip(false);
        }
        else
        {
            EnableEquip(true);
        }

    }




    private void EnableUpgrade(bool _value)
    {
        upgradeBtn.interactable = _value;
        upgradeBtn.gameObject.SetActive(true);

    }

    private void EnableEquip(bool _value)
    {
        equipBtn.interactable = _value;

    }

    public void UpdateLevelData()
    {
        UpgradeRequirement[] _requirementsArray = helmetInstance.baseHelmet.levelUpRequirements;
        string durabilityText = "";
        string statsText = "";
        for (int i = 0; i < _requirementsArray.Length; i++)
        {
            if (i > 0) durabilityText += "/";

            if (i + 1 == helmetInstance.currentLevel)
                durabilityText += $"<b><color=#FFFFFF>{_requirementsArray[i].durability}</color></b>";
            else
                durabilityText += $"<color=#888888>{_requirementsArray[i].durability}</color>";
        }

        for (int i = 0; i < _requirementsArray.Length; i++)
        {
            if (i > 0) statsText += "/";

            if (i + 1 == helmetInstance.currentLevel)
                statsText += $"<b><color=#FFFFFF>{_requirementsArray[i].powerStat}</color></b>";
            else
                statsText += $"<color=#888888>{_requirementsArray[i].powerStat}</color>";
        }

        string _desc = helmetInstance.baseHelmet.effects[0].description.Replace("{{{values}}}", statsText);
        _desc = _desc.Replace("{{{ELEMENT}}}", helmetInstance.Element.ToString());
        effDescTXT.text = _desc;
        hpTXT.text = durabilityText;
    }

    private void ColorHelmetNameGradient()
    {

        VertexGradient newGradient = new VertexGradient();
        newGradient.topLeft = Color.white;    // Left side color
        newGradient.topRight = Color.white;  // Right side color
        newGradient.bottomLeft = elementColors[(int)helmetInstance.baseHelmet.element];
        newGradient.bottomRight = elementColors[(int)helmetInstance.baseHelmet.element];

        nameTXT.colorGradient = newGradient;
        nameTXT.ForceMeshUpdate();
    }

}
