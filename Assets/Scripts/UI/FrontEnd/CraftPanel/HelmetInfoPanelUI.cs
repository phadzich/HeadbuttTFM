using System;
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
    public Image strongIcon;
    public Image res01Icon;
    public Image res02Icon;
    public GameObject res02;

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
        effNameTXT.color = UIManager.Instance.elementColors[(int)helmetInstance.baseHelmet.element];
        effCostTXT.text = (helmetInstance.baseHelmet.effects[0].hbPointsUsed + 1).ToString();

        UpdateLevelData();
        powerTXT.text = ((int)helmetInstance.baseHelmet.miningPower + 1).ToString();
        helmetIcon.gameObject.SetActive(true);
        helmetIcon.sprite = helmetInstance.baseHelmet.icon;

        UpdateStrongVsData(helmetInstance.Element);

        elementIcon.sprite = UIManager.Instance.elementIcons[(int)helmetInstance.baseHelmet.element];
        elementPanel.color = UIManager.Instance.elementColors[(int)helmetInstance.baseHelmet.element];

        effectIcon.sprite = helmetInstance.baseHelmet.effects[0].effectIcon;
        effectIconPanel.color = UIManager.Instance.elementColors[(int)helmetInstance.baseHelmet.element];

        equippedLabel.SetActive(helmetInstance.isEquipped);
        equipBtn.gameObject.SetActive(!helmetInstance.isEquipped);
    }

    private void UpdateStrongVsData(ElementType _element)
    {
        int _weakId = 0;
        switch (_element)
        {
            case ElementType.Neutral:
                {
                    _weakId = 0;
                    break;
                }
            case ElementType.Fire:
                {
                    _weakId = 4;
                    break;
                }
            case ElementType.Water:
                {
                    _weakId = 2;
                    break;
                }
            case ElementType.Grass:
                {
                    _weakId = 5;
                    break;
                }
            case ElementType.Electric:
                {
                    _weakId = 3;
                    break;
                }
        }

        strongIcon.color = elementColors[_weakId];
        strongIcon.sprite = elementIcons[_weakId];

        strongTXT.color = elementColors[_weakId];
        strongTXT.text = Enum.GetNames(typeof(ElementType))[_weakId];
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
        res01TXT.text = $"{ResourceManager.Instance.ownedResources[_requirements.requirements[0].resource]}/{_requirements.requirements[0].quantity}";

        res02.SetActive(true);

        if (_requirements.requirements.Count > 1)
        {
            res02Icon.sprite = _requirements.requirements[1].resource.icon;
            res02TXT.text = $"{ResourceManager.Instance.ownedResources[_requirements.requirements[1].resource]}/{_requirements.requirements[1].quantity}";
        }
        else
        {
            res02.SetActive(false);
        }

        if (helmetInstance.isEquipped)
        {
            EnableEquip(false);
            equipBtn.gameObject.SetActive(false);
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
        newGradient.bottomLeft = UIManager.Instance.elementColors[(int)helmetInstance.baseHelmet.element];
        newGradient.bottomRight = UIManager.Instance.elementColors[(int)helmetInstance.baseHelmet.element];

        nameTXT.colorGradient = newGradient;
        nameTXT.ForceMeshUpdate();
    }

}
