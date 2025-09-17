using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    Rigidbody rb;

    public PlayerMainStateEnum currentMainState;
    [SerializeField] public List<PlayerEffectStateEnum> currentEffects = new();

    public bool canMove;
    public bool canReceiveDamage;
    public bool canBounce;
    public bool canHeadbutt;

    public bool onMiningLvl = false;

    public bool bounceAfterStunPending = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
                canHeadbutt = false;

                break;

            case PlayerMainStateEnum.Disabled:
                canMove = false;
                canBounce = false;
                canHeadbutt = true;

                break;

            case PlayerMainStateEnum.FallingIntoMINE:
                canMove = false;
                canHeadbutt = false;
                canBounce = true;
                bounceAfterStunPending = true;
                onMiningLvl = true;

                break;

            case PlayerMainStateEnum.FallingIntoNPC:
                canMove = false;
                canHeadbutt = false;
                canBounce = true;
                onMiningLvl = false;

                break;

            case PlayerMainStateEnum.Bouncing:
                canBounce = true;
                canMove = true;
                canHeadbutt = true;

                if (bounceAfterStunPending)
                {
                    PlayerManager.Instance.playerBounce.BounceUp();
                    bounceAfterStunPending = false;
                }

                //animator.Play("Bouncing");
                break;

            case PlayerMainStateEnum.Headbutt:
                canMove = true;
                canBounce = true;
                canHeadbutt = true;

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
                canHeadbutt = false;

                // caminar sin saltar
                //animator.Play("Walk");
                break;

            case PlayerMainStateEnum.Dead:
                canBounce = false;
                canMove = false;
                canHeadbutt = false;

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
        else if (bounceAfterStunPending & !isOnState(PlayerMainStateEnum.FallingIntoMINE))
        {
            ChangeState(PlayerMainStateEnum.Bouncing);
        }

        if (currentEffects.Contains(PlayerEffectStateEnum.Damaged))
        {
            // opcional: partículas o parpadeo

            //animator.Play("Damaged");
              
        }

    }


    public void ChangeState(PlayerMainStateEnum _state)
    {
        //Debug.Log("CHANGE TO:" + _state.ToString());
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
