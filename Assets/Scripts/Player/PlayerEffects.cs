using System.Collections;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] public bool isCooldownActive => playerStates.hasEffect(PlayerEffectStateEnum.Cooldown);
    [SerializeField] private float cooldownTime;

    private PlayerStates playerStates;

    private void Start()
    {
        playerStates = GetComponent<PlayerStates>();
    }

    public void GetStunned(float _stunnedDuration)
    {
        if (!playerStates.hasEffect(PlayerEffectStateEnum.Stunned) && playerStates.canReceiveDamage) // SI PUEDE RECIBIR DAÑO
        {
            SoundManager.PlaySound(SFXType.GET_STUNN);
            PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Stunned;
            PlayerManager.Instance.playerEmojis.StunnedEmoji();
            playerStates.AddEffect(_effect);
            StartCoroutine(RemoveEffectAfterTime(_effect, _stunnedDuration));
        }
        else // SI NO PUEDE RECIBIR DAÑO (en cooldown o en shield)
        {
            if (playerStates.hasEffect(PlayerEffectStateEnum.Shield))
            {
                StartCoroutine(ShieldOff(0f));
            }
        }

    }

    public void TakeDamage(int _amount)
    {
        if (playerStates.canReceiveDamage)
        {
            HelmetManager.Instance.currentHelmet.TakeDamage(_amount);
            SoundManager.PlaySound(SFXType.RECIEVE_DAMAGE, 0.5f);

            if (!isCooldownActive) StartCoroutine(StartCooldown(cooldownTime));
        }
        else
        {
            if (playerStates.hasEffect(PlayerEffectStateEnum.Shield))
            {
                StartCoroutine(ShieldOff(0f));
            }
        }
    }

    // Shield effect

    public void ShieldOn(float _shieldDuration)
    {
        if (playerStates.hasEffect(PlayerEffectStateEnum.Shield))
            return;

        PlayerManager.Instance.ActivateShield();

        playerStates.AddEffect(PlayerEffectStateEnum.Shield);

        StartCoroutine(ShieldOff(_shieldDuration));

    }

    private IEnumerator ShieldOff(float _time)
    {
        if (_time > 0f)
            yield return new WaitForSeconds(_time);

        PlayerManager.Instance.DeactivateShield();

        playerStates.RemoveEffect(PlayerEffectStateEnum.Shield);

        if (!isCooldownActive) StartCoroutine(StartCooldown(cooldownTime));
    }

    // General functions

    private IEnumerator RemoveEffectAfterTime(PlayerEffectStateEnum _effect, float _time)
    {
        if (_time > 0f)
            yield return new WaitForSeconds(_time);

        playerStates.RemoveEffect(_effect);

        if (!isCooldownActive) StartCoroutine(StartCooldown(cooldownTime));
    }

    public IEnumerator StartCooldown(float _time)
    {
        PlayerEffectStateEnum _effect = PlayerEffectStateEnum.Cooldown;

        playerStates.AddEffect(_effect); // bloquea daño durante cooldown
        yield return new WaitForSeconds(_time);
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
