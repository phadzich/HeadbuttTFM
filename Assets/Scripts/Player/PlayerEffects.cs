using System.Collections;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] public bool isCooldownActive => playerStates.hasEffect(PlayerEffectStateEnum.Cooldown);
    [SerializeField] private float cooldownTime;

    private PlayerStates playerStates;
    private Coroutine _shieldRoutine;

    private void Start()
    {
        playerStates = GetComponent<PlayerStates>();
    }

    public void GetStunned(float _stunnedDuration)
    {
        if (playerStates.canReceiveDamage && !playerStates.hasEffect(PlayerEffectStateEnum.Stunned)) // SI PUEDE RECIBIR DAÑO
        {
            PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Stunned;
            playerStates.AddEffect(_effect);
            StartCoroutine(RemoveEffectAfterTime(_effect, _stunnedDuration));
        }
        else // SI NO PUEDE RECIBIR DAÑO (en cooldown o en shield)
        {
            if (playerStates.hasEffect(PlayerEffectStateEnum.Shield))
            {
                ShieldOff();
            }
        }

    }

    public bool ShieldOn(float _shieldDuration)
    {
        if (_shieldRoutine != null)
            return false;

        PlayerManager.Instance.ActivateShield();

        PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Shield;
        playerStates.AddEffect(_effect);

        _shieldRoutine = StartCoroutine(RemoveEffectAfterTime(_effect, _shieldDuration));

        return true;
    }

    public void ShieldOff()
    {
        PlayerManager.Instance.DeactivateShield();
        PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Shield;

        if (_shieldRoutine != null)
        {
            StopCoroutine(_shieldRoutine);
            _shieldRoutine = null;
        }

        StartCoroutine(RemoveEffectAfterTime(_effect, 0f));
    }

    public void TakeDamage(int _amount)
    {
        if (playerStates.canReceiveDamage)
        {
            HelmetManager.Instance.currentHelmet.TakeDamage(_amount);

            if (!isCooldownActive) StartCoroutine(StartCooldown());
        }
        else
        {
            if (playerStates.hasEffect(PlayerEffectStateEnum.Shield))
            {
                ShieldOff();
            }
        }
    }

    private IEnumerator RemoveEffectAfterTime(PlayerEffectStateEnum _effect, float _time)
    {
        if (_time > 0f)
            yield return new WaitForSeconds(_time);

        playerStates.RemoveEffect(_effect);

        if (!isCooldownActive) StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Cooldown;

        playerStates.AddEffect(_effect); // bloquea daño durante cooldown
        yield return new WaitForSeconds(cooldownTime);
        playerStates.RemoveEffect(_effect);  // desbloquea daño
    }

}


public enum PlayerEffectStateEnum
{
    Cooldown,
    Shield,
    Stunned,
    Damaged
}
