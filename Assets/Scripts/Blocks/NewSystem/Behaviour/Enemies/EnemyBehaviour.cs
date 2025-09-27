using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IBlockBehaviour
{
    public AudioClip onBounce;

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        PlayOnBounce();
        MatchManager.Instance.FloorBounced();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        PlayOnBounce();
        MatchManager.Instance.FloorBounced();
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }

    private void PlayOnBounce()
    {
        if (onBounce != null) SoundManager.PlaySound(SFXType.ENEMY, 1f, onBounce);
    }
}
