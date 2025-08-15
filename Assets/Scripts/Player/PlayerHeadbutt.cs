using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeadbutt : MonoBehaviour
{
    Rigidbody rb;
    PlayerStates playerStates;
    public GameObject bodyMesh;

    [SerializeField] public float maxHBpoints;
    [SerializeField] public float currentHBpoints;
    public bool hasMaxHBPoints => currentHBpoints==maxHBpoints;
[Header("HEADBUTT CONFIG")]
    [SerializeField]
    float headbuttCooldown;
    [SerializeField]
    bool headbuttOnCooldown;
    [SerializeField]
    float headbuttPower;

    [Header("HEADBUTT CHECKS")]
    [SerializeField]
    float timeSinceLastHeadbutt;
    CinemachineImpulseSource impulseSource;

    public Action<float, float> onHBPointsChanged;
    [Header("POTION VALUES")]
    [SerializeField] public List<int> potionValues;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        UpdateHeadbuttCooldown();
        KeepCentered();
    }

    public void UseHBPotion(int _potionID)
    {
        AddHBPoints(potionValues[_potionID]);
    }

    public void AddHBPoints(float _amount)
    {
        if(_amount>maxHBpoints - currentHBpoints)
        {
            ChangeHBpoints(maxHBpoints - currentHBpoints);
        }
        else
        {
            ChangeHBpoints(_amount);
        }

    }

    public bool TryUseHBPoints(float _amount)
    {
        bool _result = false;

        if (_amount > currentHBpoints)
        {
            _result = false;
            Debug.Log("NOT ENOUGH HB POINTS");
        }
        else
        {
            _result = true;
            UseHBPoints(_amount);
        }

            return _result;
    }

    public void UseHBPoints(float _amount)
    {
        ChangeHBpoints(-_amount);
    }

    public void ChangeHBpoints(float _amount)
    {
        currentHBpoints += _amount;
        onHBPointsChanged?.Invoke(currentHBpoints,maxHBpoints);
    }


    private void KeepCentered()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
    }



    public void Headbutt(InputAction.CallbackContext context)
    {
        if(LevelManager.Instance.currentSublevel.config is MiningSublevelConfig)
        {
            // por ahora quite bounceDirection == "DOWN" && 
            if (context.phase == InputActionPhase.Performed)
            {
                
                if (!headbuttOnCooldown &&
                    TryUseHBPoints(1) &&
                    PlayerManager.Instance.playerMovement.blockNSBelow != null)
                {
                    HeadbuttUp();
                }
            }
        }else if(LevelManager.Instance.currentSublevel.config is NPCSublevelConfig){
            Debug.Log("NO HAY HBS EN NPC");
        }
    }

    public void HeadbuttUp()
    {
        
        rb.transform.position = PlayerManager.Instance.playerMovement.blockNSBelow.transform.position+new Vector3(0,2f,0);
        rb.linearVelocity = new Vector3(0, headbuttPower, 0);

        PlayerManager.Instance.playerMovement.blockNSBelow.OnHeadbutt(HelmetManager.Instance.currentHelmet);
        ScreenShake();
        RestartHeadbuttCooldown();
        PlayerManager.Instance.playerAnimations.HeadbuttSS();
        HelmetManager.Instance.currentHelmet.OnHeadbutt();
    }

    private void UpdateHeadbuttCooldown()
    {
        timeSinceLastHeadbutt += Time.deltaTime;
        if (timeSinceLastHeadbutt <= headbuttCooldown)
        {
            headbuttOnCooldown = true;
        }
        else
        {
            headbuttOnCooldown = false;
        }

    }

    private void RestartHeadbuttCooldown()
    {
        timeSinceLastHeadbutt = 0;

    }

    private void ScreenShake()
    {
        // 0 = shake activado, 1 = shake desactivado
        if (PlayerPrefs.GetInt("CameraShakeEnabled", 1) == 0)
        {
            impulseSource.GenerateImpulse();
        }
    }



}
