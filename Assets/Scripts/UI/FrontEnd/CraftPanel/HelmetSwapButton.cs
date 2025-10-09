using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelmetSwapButton : MonoBehaviour
{
    public Image helmetIcon;
    private HelmetInstance helmet;
    public TextMeshProUGUI duraTXT;
    public TextMeshProUGUI powerTXT;
    public Image powerPanel;
    public Image powerIcon;

    // Se crea el prefab con la información del blueprint
    public void SetUp(HelmetInstance helmetI)
    {
        helmet = helmetI;
        helmetIcon.sprite = helmetI.baseHelmet.icon;
        duraTXT.text = helmetI.durability.ToString();
        powerTXT.text = ((int)helmetI.baseHelmet.miningPower+1).ToString();
        powerPanel.color = UIManager.Instance.elementColors[(int)helmetI.Element];
        powerIcon.sprite = UIManager.Instance.elementIcons[(int)helmetI.Element];
        GetComponent<Button>().interactable = false;
    }

   
    public void OnClickSelectBtn()
    {
        HelmetManager.Instance.SwapHelmet(CraftingManager.Instance.selectedHelmet,helmet);
        UIManager.Instance.craftingPanel.swapHelmetsUI.UpdateHelmetList();
        UIManager.Instance.craftingPanel.ToggleSwapPanel(false);
    }


}
