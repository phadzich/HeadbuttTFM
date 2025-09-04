using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/BasicAttackEffectData")]
public class BasicAttackEffectData : HelmetEffectData
{
    public GameObject effectParticles;
    public GameObject effectArea;

    [SerializeField] public LayerMask enemyLayer;

    public override HelmetEffect CreateEffect()
    {
        return new BasicAttack(this);
    }
}