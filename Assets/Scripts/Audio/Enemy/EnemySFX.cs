using System.Collections;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [Header("Clips de Sonido")]
    public AudioClip idleClip;
    public AudioClip attackClip;
    public AudioClip damageClip;
    public AudioClip deathClip;

    private Coroutine idleRoutine;

    private void Start()
    {
        PlayIdle();
    }

    private void PlayIdle()
    {
        SoundManager.PlaySound(SFXType.ENEMY, transform.position, idleClip, 1f,true);
    }

    public void PlayAttack(float _volume = 1f)
    {
        if (attackClip == null) return;
        StartCoroutine(PlayAndResumeIdle(attackClip, _volume));
    }

    public void PlayDamage()
    {
        if (damageClip == null) return;
        StartCoroutine(PlayAndResumeIdle(damageClip));
    }

    public void PlayDeath()
    {
        if (deathClip == null) return;
        SoundManager.PlaySound(SFXType.ENEMY, transform.position, deathClip);
    }

    IEnumerator PlayAndResumeIdle(AudioClip _clip, float _volume = 1f)
    {
        SoundManager.PlaySound(SFXType.ENEMY, transform.position, _clip, _volume);
        yield return new WaitForSeconds(_clip.length);
        PlayIdle();
    }


}
