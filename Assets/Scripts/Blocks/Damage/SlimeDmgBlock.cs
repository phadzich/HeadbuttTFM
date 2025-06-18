using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SlimeDmgBlock : DamageBlock
{

    private void Start()
    {
    }
    public override void Bounce()
    {

        if (HelmetManager.Instance.currentHelmet.helmetEffect != helmetCounter)
        {
            PushPlayerRandomly();
        }

    }

    private void PlayBounceSound()
    {
        audioSource.PlayOneShot(damageSound, 0.7f);
    }

    public override void Headbutt()
    {
        if (HelmetManager.Instance.currentHelmet.helmetEffect != helmetCounter)
        {
            PushPlayerRandomly();
        }
    }
}
