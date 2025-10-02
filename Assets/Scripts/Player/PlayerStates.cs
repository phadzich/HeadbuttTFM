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
    public bool interruptHeadbutt;

    public bool onMiningLvl = false;

    public bool bounceAfterStunPending = false;

    private Coroutine deathCoroutine = null;

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
                canReceiveDamage = false;
                interruptHeadbutt = false;

                break;

            case PlayerMainStateEnum.Disabled:
                canMove = false;
                canBounce = false;
                canHeadbutt = true;
                canReceiveDamage = true;
                interruptHeadbutt = true;
                bounceAfterStunPending = true;

                break;

            case PlayerMainStateEnum.FallingIntoMINE:
                canMove = false;
                canHeadbutt = false;
                canBounce = true;
                onMiningLvl = true;
                canReceiveDamage = false;
                interruptHeadbutt = false;

                break;

            case PlayerMainStateEnum.FallingIntoNPC:
                canMove = false;
                canHeadbutt = false;
                canBounce = true;
                onMiningLvl = false;
                canReceiveDamage = false;
                interruptHeadbutt = false;

                break;

            case PlayerMainStateEnum.Bouncing:
                canBounce = true;
                canMove = true;
                canHeadbutt = true;
                canReceiveDamage = true;
                interruptHeadbutt = false;

                //animator.Play("Bouncing");
                break;

            case PlayerMainStateEnum.Headbutt:
                canMove = true;
                canBounce = true;
                canHeadbutt = true;
                canReceiveDamage = true;
                interruptHeadbutt = false;

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
                canReceiveDamage = false;
                interruptHeadbutt = false;

                // caminar sin saltar
                //animator.Play("Walk");
                break;

            case PlayerMainStateEnum.Dead:
                canBounce = false;
                canMove = false;
                canHeadbutt = false;
                canReceiveDamage = false;
                interruptHeadbutt = true;

                //ANIMACION

                if (deathCoroutine == null)
                {
                    deathCoroutine = StartCoroutine(StartGameOverSequence(2f));
                }
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
        }


        if (bounceAfterStunPending && !hasEffect(PlayerEffectStateEnum.Stunned))
        {
            bounceAfterStunPending = false;
            PlayerManager.Instance.playerBounce.BounceUp();
            ChangeState(PlayerMainStateEnum.Bouncing);
        }

        if (currentEffects.Contains(PlayerEffectStateEnum.Damaged))
        {
            // opcional: partículas o parpadeo

            //animator.Play("Damaged");
              
        }

    }

    private IEnumerator StartGameOverSequence(float _time)
    {
        yield return new WaitForSeconds(_time);

        MatchManager.Instance.RestartMatches();

        //AQUI AGREGAR CODIGO PARA MOSTRAR SCREEN DE GAME OVER 

        UIManager.Instance.ShowGameOver();

    }

    public void CleanDeathCoroutine()
    {
        deathCoroutine = null;
    }


    public void ChangeState(PlayerMainStateEnum _state)
    {
        if (currentMainState != _state)
        {
            //Debug.Log("CHANGE TO:" + _state.ToString());
            currentMainState = _state;
        }

        if (currentMainState == PlayerMainStateEnum.Dead)
        {
            // Avisar al Headbutt que se interrumpa
            var headbutt = PlayerManager.Instance.playerHeadbutt;
            if (headbutt != null)
            {
                headbutt.InterruptHeadbutt();
            }
        }
    }

    public void AddEffect(PlayerEffectStateEnum _effect)
    {
        //Debug.Log("ADD EFFECT:" + _effect.ToString());
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
