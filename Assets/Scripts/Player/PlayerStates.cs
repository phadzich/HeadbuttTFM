using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{

    public PlayerMainStateEnum currentMainState;
    [SerializeField] public List<PlayerEffectStateEnum> currentEffects = new();

    public bool canMove;
    public bool canReceiveDamage;
    public bool canBounce;

    public bool bounceAfterStunPending = false;

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
            case PlayerMainStateEnum.Idle:
                canMove = true;
                canBounce = false;
                break;

            case PlayerMainStateEnum.Falling:
                canMove = false;
                break;

            case PlayerMainStateEnum.Disabled:
                canBounce = false;
                canMove = false;
                break;

            case PlayerMainStateEnum.Bouncing:
                canBounce = true;
                canMove = true;

                if (bounceAfterStunPending)
                {
                    PlayerManager.Instance.playerBounce.BounceUp();
                    bounceAfterStunPending = false;
                }

                //animator.Play("Bouncing");
                break;

            case PlayerMainStateEnum.Headbutt:
                canMove = true;

                if (hasEffect(PlayerEffectStateEnum.Stunned))
                {
                    bounceAfterStunPending = false;
                    RemoveEffect(PlayerEffectStateEnum.Stunned);
                }

                //animator.Play("Headbutt");
                break;

            case PlayerMainStateEnum.Walk:
                canBounce = false;
                canMove = true;
                // caminar sin saltar
                //animator.Play("Walk");
                break;

            case PlayerMainStateEnum.Dead:
                canBounce = false;
                canMove = false;

                // jugador muerto
                //animator.Play("Dead");
                break;
        }
    }

    void HandleEffects()
    {
        if (hasEffect(PlayerEffectStateEnum.Shield) || hasEffect(PlayerEffectStateEnum.Cooldown))
        {
            canReceiveDamage = false;
        }
        else
        {
            canReceiveDamage = true;
        }

        if (hasEffect(PlayerEffectStateEnum.Stunned))
        {
            ChangeState(PlayerMainStateEnum.Disabled);
            bounceAfterStunPending = true;
        }
        else if (bounceAfterStunPending)
        {
            ChangeState(PlayerMainStateEnum.Bouncing);
            canMove = true;
        }

        if (currentEffects.Contains(PlayerEffectStateEnum.Damaged))
        {
            // opcional: partículas o parpadeo

            //animator.Play("Damaged");
              
        }

        // puedes agregar más efectos aquí
    }

    public void ChangeState(PlayerMainStateEnum _state)
    {
        currentMainState = _state;
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
