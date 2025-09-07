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
        effDescTXT.text = helmetInstance.baseHelmet.effects[0].description;
        effCostTXT.text = (helmetInstance.baseHelmet.effects[0].hbPointsUsed + 1).ToString();

        UpdateHealth();
        powerTXT.text = ((int)helmetInstance.baseHelmet.miningPower + 1).ToString();

        helmetIcon.sprite = helmetInstance.baseHelmet.icon;


        elementIcon.sprite = elementIcons[(int)helmetInstance.baseHelmet.element];
        elementPanel.color = elementColors[(int)helmetInstance.baseHelmet.element];

        effectIcon.sprite = helmetInstance.baseHelmet.effects[0].effectIcon;
        effectIconPanel.color = elementColors[(int)helmetInstance.baseHelmet.element];

    }

    private void UpdateUpgradeButton()
    {
        UpgradeRequirement _requirements = helmetInstance.baseHelmet.levelUpRequirements[helmetInstance.nextLevel];

        if (helmetInstance.currentLevel == 0)
        {
            nextAction = "CRAFT!";
        }
        else if (helmetInstance.currentLevel == 3)
        {
            nextAction = "MAXED!";
        }
        else
        {
            nextAction = "LEVEL UP!";
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

        if (helmetInstance.isDiscovered)
        {
            upgradeBtn.interactable = true;
            if (helmetInstance.isCrafted)
            {
                equipBtn.interactable = true;
            }
            else
            {
                equipBtn.interactable = false;
            }
        }
        else
        {
            upgradeBtn.interactable = false;
            equipBtn.interactable = false;
        }
    }

    public void UpdateHealth()
    {
        UpgradeRequirement[] _requirementsArray = helmetInstance.baseHelmet.levelUpRequirements;
        string valuesText = "";

        for (int i = 0; i < _requirementsArray.Length; i++)
        {
            if (i > 0) valuesText += "/";

            if (i + 1 == helmetInstance.currentLevel)
                valuesText += $"<b><color=#FFFFFF>{_requirementsArray[i].durability}</color></b>";
            else
                valuesText += $"<color=#888888>{_requirementsArray[i].durability}</color>";
        }

        hpTXT.text = valuesText;
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
