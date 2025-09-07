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
        //Debug.Log("IDLE SOUND");
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
            SoundManager.PlaySound(SFXType.ENEMY, transform.position, idleClip, 0.2f);
            //SoundManager.PlaySound3D(idleSound, transform.position,0.2f);
        }
    }

    public void PlayAttack()
    {
        //Debug.Log("DIE ATTACK");
        StopIdle();
        StartCoroutine(PlayAndResumeIdle(attackClip));
    }

    public void PlayDamage()
    {
        //Debug.Log("DAMAGE SOUND");
        StopIdle();
        StartCoroutine(PlayAndResumeIdle(damageClip));
    }

    public void PlayDeath()
    {
        //Debug.Log("DIE SOUND");
        StopIdle();
        SoundManager.PlaySound(SFXType.ENEMY, transform.position, deathClip, 0.2f);
    }

    IEnumerator PlayAndResumeIdle(AudioClip _clip)
    {
        SoundManager.PlaySound(SFXType.ENEMY, transform.position, _clip, 0.2f);
        yield return new WaitForSeconds(_clip.length);
        PlayIdle();
    }


}
