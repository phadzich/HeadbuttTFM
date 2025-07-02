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

    }

    private void PlayBounceSound()
    {
        audioSource.PlayOneShot(damageSound, 0.7f);
    }

    public override void OnHeadbutted(HelmetInstance _helmetInstance)
    {
        PushPlayerRandomly();
    }
}
