using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/FireBreathEffectData")]
public class FireBreathEffectData : HelmetEffectData
{
    public Vector3 damageArea;
    public int damage;
    public int hbPointsUsed;
    [SerializeField] public LayerMask enemyLayer;

    public override HelmetEffect CreateEffect()
    {
        return new FireBreath(this);
    }
}