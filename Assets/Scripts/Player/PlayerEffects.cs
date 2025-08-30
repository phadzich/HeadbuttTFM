using System.Collections;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] public bool isCooldownActive = false;
    [SerializeField] private float cooldownTime;

    private PlayerStates playerStates => PlayerManager.Instance.playerStates;
    private Coroutine _shieldRoutine;

    public void GetStunned(float _stunnedDuration)
    {
        if (playerStates.canReceiveDamage) // SI PUEDE RECIBIR DAÑO
        {
            PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Stunned;
            playerStates.AddEffect(_effect);
            StartCoroutine(RemoveEffectAfterTime(_effect, _stunnedDuration));
        }
        else // SI NO PUEDE RECIBIR DAÑO (en cooldown o en shield)
        {
            Debug.Log("NO PUEDE RECIBIR DAMAGE");
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

        PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Shield;
        playerStates.AddEffect(_effect);

        _shieldRoutine = StartCoroutine(RemoveEffectAfterTime(_effect, _shieldDuration));

        return true;
    }

    public void ShieldOff()
    {
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
        Debug.Log("EN COOLDOWN");
        isCooldownActive = true;
        playerStates.canReceiveDamage = false; // bloquea daño durante cooldown
        Debug.Log($"can {playerStates.canReceiveDamage}");
        yield return new WaitForSeconds(cooldownTime);
        playerStates.canReceiveDamage = true;  // desbloquea daño
        Debug.Log($"can {playerStates.canReceiveDamage}");
        isCooldownActive = false;
    }

}


public enum PlayerEffectStateEnum
{
    Shield,
    Stunned,
    Damaged
}
