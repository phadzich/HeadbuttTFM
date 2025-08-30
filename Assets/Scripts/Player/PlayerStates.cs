using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    public PlayerMainStateEnum currentMainState;
    public List<PlayerEffectStateEnum> currentEffects { get; private set; } = new();

    public bool canMove;
    public bool canReceiveDamage;
    private bool bounceAfterStunPending = false;

    private void Start()
    {
        canMove = true;
        canReceiveDamage = true;
    }

    void Update()
    {
        HandleMainState();
        HandleEffects();
    }

    void HandleMainState()
    {
        switch (currentMainState)
        {
            case PlayerMainStateEnum.Bouncing:

                if (bounceAfterStunPending)
                {
                    PlayerManager.Instance.playerBounce.BounceUp();
                    bounceAfterStunPending = false;
                }
                //animator.Play("Bouncing");
                break;

            case PlayerMainStateEnum.Headbutt:
                // lógica de salto alto
                //animator.Play("Headbutt");
                break;

            case PlayerMainStateEnum.Walk:
                // caminar sin saltar
                //animator.Play("Walk");
                break;

            case PlayerMainStateEnum.Dead:
                // jugador muerto
                //animator.Play("Dead");
                break;
        }
    }

    void HandleEffects()
    {
        if (currentEffects.Contains(PlayerEffectStateEnum.Shield))
        {

            canReceiveDamage = false;
            PlayerManager.Instance.ActivateShield();
        }
        else
        {
            canReceiveDamage = true;
            PlayerManager.Instance.DeactivateShield();
        }

        if (currentEffects.Contains(PlayerEffectStateEnum.Stunned))
        {
            currentMainState = PlayerMainStateEnum.None;
            canMove = false;
            bounceAfterStunPending = true;
        }
        else
        {
            currentMainState = PlayerMainStateEnum.Bouncing;
            canMove = true;
        }

        if (currentEffects.Contains(PlayerEffectStateEnum.Damaged))
        {
            // opcional: partículas o parpadeo

            //animator.Play("Damaged");
              
        }

        // puedes agregar más efectos aquí
    }

    public void AddEffect(PlayerEffectStateEnum _effect)
    {
        currentEffects.Add(_effect);
    }

    public void RemoveEffect(PlayerEffectStateEnum _effect)
    {
        currentEffects.Remove(_effect);
    }

    public bool hasEffect(PlayerEffectStateEnum _effect)
    {
        return currentEffects.Contains(_effect);
    }

    public bool isOnState(PlayerMainStateEnum _state)
    {
        return currentMainState == _state;
    }



}
