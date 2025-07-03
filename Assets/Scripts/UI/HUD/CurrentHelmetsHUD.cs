using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CurrentHelmetsHUD : MonoBehaviour
{
    [Header("Current Helmet Info")]
    [SerializeField] public Image currentIcon;
    [SerializeField] public HelmetInstance currentHelmetInstance;
    [SerializeField] public HelmetHUD prevHelmetHUD;
    [SerializeField] public HelmetHUD currentHelmetHUD;

    [Header("EquippedHelmets")]
    [SerializeField] Transform helmetsContainer;
    public GameObject helmetHUDPF;
    public List<HelmetHUD> equippedHelmetHUDs;

    public void EquipHelmet(HelmetInstance _helmInstance)
    {
        var _newHelmetHUDPF = Instantiate(helmetHUDPF, helmetsContainer);
        HelmetHUD _newHelmet = _newHelmetHUDPF.GetComponent<HelmetHUD>();
        _newHelmet.LoadHelmet(_helmInstance);
        equippedHelmetHUDs.Add(_newHelmet);
    }
    public void WearNewHelmet(HelmetInstance _helmIntance)
    {
        prevHelmetHUD = currentHelmetHUD;

        //UNWEAR o DISABLE EL ANTERIOR
        if (prevHelmetHUD != null)
        {
            if (prevHelmetHUD.helmetInstance.IsWornOut)
            {
                prevHelmetHUD.Broken();
            }
            else
            {
                prevHelmetHUD.UnWearHelmet();
            }

        }

        //WEAR EL NUEVO
        currentHelmetInstance = _helmIntance;
        currentHelmetHUD = FindHUDbyInstance(_helmIntance);
        currentHelmetHUD.WearHelmet();
    }

    private HelmetHUD FindHUDbyInstance(HelmetInstance _instance)
    {
        foreach(HelmetHUD _hud in equippedHelmetHUDs)
        {
            if( _hud.helmetInstance == _instance)
            {
                return _hud;
            }
        }

        return null;
    }

}
