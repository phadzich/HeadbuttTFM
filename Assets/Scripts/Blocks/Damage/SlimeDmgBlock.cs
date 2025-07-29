using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SlimeDmgBlock : DamageBlock
{

    private void Start()
    {
    }
    public override void OnBounced(HelmetInstance _helmetInstance)
    {

        PushPlayerRandomly();
        SoundManager.PlaySound(SoundType.PUSHDAMAGE, 0.7f);
    }

    private void PlayBounceSound()
    {
        SoundManager.PlaySound(SoundType.PUSHDAMAGE, 0.7f);
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        PushPlayerRandomly();
        SoundManager.PlaySound(SoundType.PUSHDAMAGE, 0.7f);
    }
}
