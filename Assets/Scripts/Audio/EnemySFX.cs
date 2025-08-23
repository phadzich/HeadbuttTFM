using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySFX : MonoBehaviour
{
    [Header("Clips de Sonido")]
    public AudioClip idleSound;
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    private Coroutine idleRoutine;

    private AudioSource audioSource;

    void Awake()
    {
        // Busca o crea un AudioSource en el mismo GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayIdle();
    }

    private void PlayIdle()
    {
        Debug.Log("IDLE SOUND");
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
            PlayClip(idleSound);
        }
    }

    public void PlayAttack()
    {
        StopIdle();
        StartCoroutine(PlayAndResumeIdle(attackSound));
    }

    public void PlayDamage()
    {
        Debug.Log("DAMAGE SOUND");
        StopIdle();
        StartCoroutine(PlayAndResumeIdle(hurtSound));
    }

    public void PlayDeath()
    {
        Debug.Log("DIE SOUND");
        StopIdle();
        PlayClip(deathSound);
    }

    IEnumerator PlayAndResumeIdle(AudioClip _clip)
    {
        PlayClip(_clip);
        yield return new WaitForSeconds(_clip.length);
        PlayIdle();
    }

    private void PlayClip(AudioClip _clip)
    {
        if (_clip != null)
            audioSource.PlayOneShot(_clip);
    }

}
