using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shield : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private bool isOnCooldown;
    private float duration;

    public void Setup(float _duration)
    {
        duration = _duration;
    }

    public void Activate()
    {
        isActive = true;
        gameObject.SetActive(true);
        StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
    }

    public void Deactivate()
    {
        if (!isActive) return;

        isActive = false;
        StartCoroutine(CooldownRoutine(5f)); // Ejemplo: 5 seg de cooldown
    }

    private IEnumerator CooldownRoutine(float cooldown)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
        gameObject.SetActive(false);
        PlayerManager.Instance.activeShield = null;
        Debug.Log("SE VA A DESTRUIIR");
        Destroy(gameObject);
    }

    public bool CanBlockDamage() => isActive || isOnCooldown;
}
