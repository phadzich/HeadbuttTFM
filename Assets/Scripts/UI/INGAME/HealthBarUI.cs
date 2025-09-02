using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillBar;
    public GameObject damagePanel;
    public TextMeshProUGUI damageText;

    public Color fireColor;
    public Color neutralColor;
    public Color waterColor;
    public Color electricColor;
    public Color grassColor;

    public void UpdateBar(float _curr, float _max)
    {
        if (_curr <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            float _progress = _curr / _max;
            fillBar.fillAmount = _progress;
        }        
    }

    public void PopDamage(int _damage, ElementType _element)
    {
        damageText.text = _damage.ToString();
        damagePanel.GetComponent<Image>().color = GetElementColor(_element);
        Tween.Scale(damagePanel.transform,startValue:Vector3.zero,endValue:Vector3.one, ease:Ease.OutBounce, duration:.8f).OnComplete(HideDamage); ;
    }

    private void HideDamage()
    {
        Tween.Scale(damagePanel.transform, startValue: Vector3.one, endValue: Vector3.zero, ease: Ease.InBounce, duration: .4f);
    }

    private Color GetElementColor(ElementType _element)
    {
        Color _color = Color.white;
            switch (_element)
            {
                case ElementType.Water: return waterColor;
                case ElementType.Electric: return electricColor;
                case ElementType.Grass: return grassColor;
                case ElementType.Fire: return fireColor;
                case ElementType.Neutral: return neutralColor;
            }

        return _color;
        }
}
