using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/FireBreathEffectData")]
public class FireBreathEffectData : HelmetEffectData
{
    public Vector3 damageArea;
    public int damage;


    public GameObject effectParticles;
    public GameObject effectArea;

    [SerializeField] public LayerMask enemyLayer;

    public override HelmetEffect CreateEffect()
    {
        return new FireBreath(this);
    }
}