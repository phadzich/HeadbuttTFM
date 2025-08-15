using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Effects/WaterSplashEffectData")]
public class WaterSplashEffectData : HelmetEffectData
{
    public Vector3 damageArea;
    public int damage;
    public int hbPointsUsed;
    public GameObject effectParticles;
    public GameObject effectArea;

    [SerializeField] public LayerMask enemyLayer;

    public override HelmetEffect CreateEffect()
    {
        return new WaterSplash(this);
    }
}