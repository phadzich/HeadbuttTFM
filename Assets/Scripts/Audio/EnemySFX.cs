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
        if (idleRoutine == null) // solo iniciar si no hay una activa
            idleRoutine = StartCoroutine(IdleSoundRoutine());
    }

    private void StopIdle()
    {
        if (idleRoutine != null)
        {
            StopCoroutine(idleRoutine);
            idleRoutine = null;
        }
    }

    IEnumerator IdleSoundRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            SoundManager.PlaySound(SFXType.ENEMY, transform.position, idleClip);
        }
    }

    public void PlayAttack()
    {
        StopIdle();
        StartCoroutine(PlayAndResumeIdle(attackClip));
    }

    public void PlayDamage()
    {
        StopIdle();
        StartCoroutine(PlayAndResumeIdle(damageClip));
    }

    public void PlayDeath()
    {
        StopIdle();
        SoundManager.PlaySound(SFXType.ENEMY, transform.position, deathClip);
    }

    IEnumerator PlayAndResumeIdle(AudioClip _clip)
    {
        SoundManager.PlaySound(SFXType.ENEMY, transform.position, _clip);
        yield return new WaitForSeconds(_clip.length);
        PlayIdle();
    }


}
