using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootItemUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(LootBase _loot)
    {
        iconImage.sprite = _loot.GetIcon();
        amountText.text = _loot.amount.ToString();
    }

    public void PlayAppearAnimation()
    {
        // opcional: animación de escala, fade, etc.
    }
}
