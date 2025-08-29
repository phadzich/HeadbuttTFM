using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/CrossAttackEffectData")]
public class CrossAttackEffectData : HelmetEffectData
{
    public int crossRange;

    public GameObject effectParticles;
    public GameObject effectArea;

    [SerializeField] public LayerMask enemyLayer;

    public override HelmetEffect CreateEffect()
    {
        return new CrossAttackEffect(this);
    }
}