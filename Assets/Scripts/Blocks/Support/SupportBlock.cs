using UnityEngine;

public class SupportBlock : Block
{
    public ParticleSystem damageParticles;

    public void SetupBlock(int _subId, int _xPos, int _yPos)
    {
        sublevelId = _subId;
        sublevelPosition = new Vector2(_xPos, _yPos);
        isWalkable = true;
    }

    public override void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        damageParticles.Play();
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
        damageParticles.Play();
    }
}
