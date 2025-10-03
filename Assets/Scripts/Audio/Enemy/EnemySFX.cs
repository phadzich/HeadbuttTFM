using System.Collections;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [Header("Clips de Sonido")]
    public AudioClip idleClip;
    public AudioClip attackClip;
    public AudioClip damageClip;
    public AudioClip deathClip;

    private AudioSource source;

    private Coroutine idleRoutine;

    private bool alreadyHasAudio;


    private void Start()
    {
        alreadyHasAudio = false;
        PlayIdle();
    }

    private void PlayIdle()
    {
        if (idleClip != null && !alreadyHasAudio)
        {
            source = SoundManager.PlayEnemyIdle(transform.position, idleClip, _loop: true, _transform: gameObject);
            alreadyHasAudio = true;
        }
        else if(alreadyHasAudio)
        {
            source.Play();
        }
    }

    private void StopIdle()
    {
        if (!alreadyHasAudio) return;

        source.Stop();
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
        StopIdle();
        if (deathClip == null) return;
        SoundManager.PlayEnemySound(transform.position, deathClip);
    }

    IEnumerator PlayAndResumeIdle(AudioClip _clip, float _volume = 1f)
    {
        StopIdle();
        SoundManager.PlayEnemySound(transform.position, _clip);
        yield return new WaitForSeconds(_clip.length);
        PlayIdle();
    }


}
