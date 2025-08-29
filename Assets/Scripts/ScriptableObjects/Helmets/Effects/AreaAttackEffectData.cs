using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/FireBreathEffectData")]
public class AreaAttackEffectData : HelmetEffectData
{
    public Vector3 damageArea;

    public GameObject effectParticles;
    public GameObject effectArea;

    [SerializeField] public LayerMask enemyLayer;

    public override HelmetEffect CreateEffect()
    {
        return new AreaAttackEffect(this);
    }
}