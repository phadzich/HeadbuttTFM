using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using PrimeTween;

public class LootPopupUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private LootItemUI lootItemPrefab;

    public void ShowLoot(List<LootBase> _loots)
    {
        // Crear nuevo contenido
        foreach (var _loot in _loots)
        {
            var _itemUI = Instantiate(lootItemPrefab, contentParent);
            _itemUI.Setup(_loot);
            _itemUI.PlayAppearAnimation();
        }

        gameObject.SetActive(true);
        PopIn();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void PopIn()
    {
        Tween.Scale(this.transform, duration: .5f, endValue: Vector3.one, startValue: Vector3.zero, ease: Ease.OutBack);
        PopOut(2);
    }

    private void PopOut(float _delay)
    {
        Tween.Scale(this.transform, duration: .5f, endValue: Vector3.zero, startValue: Vector3.one, ease: Ease.InBack,startDelay:_delay).OnComplete(Hide);
    }
}