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
        ToggleExtraInfo(false);
    }

    public void ReplaceHelmet(HelmetInstance _helmInstance, int _index)
    {
        var _currentGO = helmetsContainer.GetChild(_index);

        Vector3 posicion = _currentGO.position;
        Quaternion rotacion = _currentGO.rotation;

        // Eliminar el hijo actual
        Destroy(_currentGO.gameObject);

        // Instanciar el nuevo objeto en el mismo lugar
        GameObject _newHelmetHUDPF = Instantiate(helmetHUDPF, posicion, rotacion);

        // Hacer que el nuevo objeto sea hijo del padre
        _newHelmetHUDPF.transform.SetParent(helmetsContainer);

        // Opcional: colocar al nuevo hijo en el mismo índice en la jerarquía visual
        _newHelmetHUDPF.transform.SetSiblingIndex(_index);

        HelmetHUD _newHelmet = _newHelmetHUDPF.GetComponent<HelmetHUD>();
        _newHelmet.LoadHelmet(_helmInstance);  
        equippedHelmetHUDs[_index]=_newHelmet;

        ToggleExtraInfo(false);
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

    public void UpdateUpgradePanels()
    {
        foreach (HelmetHUD _hud in equippedHelmetHUDs)
        {
            _hud.UpdateResourcesNeeded();
        }
    }

    public void ToggleExtraInfo(bool _condition)
    {
        foreach (HelmetHUD _hud in equippedHelmetHUDs)
        {
            _hud.evolvePanel.SetActive(_condition);
        }
    }

    public HelmetHUD FindHUDbyInstance(HelmetInstance _instance)
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

    public void RefreshAllHelmets()
    {
        foreach(HelmetHUD _hud in equippedHelmetHUDs)
        {
            _hud.UpdateDurability(_hud.helmetInstance.currentDurability, _hud.helmetInstance.durability);
            _hud.UnBroken();
        }
    }

}
