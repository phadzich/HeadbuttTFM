using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelmetManager : MonoBehaviour
{

    public static HelmetManager Instance;

    [Header("HELMETS")]
    public List<HelmetData> allHelmetData;
    public List<HelmetInstance> allHelmets;
    public List<HelmetInstance> helmetsEquipped = new();

    [Header("Stats")]
    public int maxEquippedHelmets = 3;
    public bool HasHelmetsLeft => helmetsEquipped.Count(helmet => !helmet.IsWornOut) >= 1;

    [Header("CURRENT HELMET")]
    public HelmetInstance currentHelmet;
    public HelmetMesh currentMesh;
    public int helmetIndex = 0;

    [Header("POTION VALUES")]
    [SerializeField] public List<int> potionValues;

    public Action<HelmetInstance> onHelmetEquipped;
    public Action<int> onHelmetsSwapped;

    //public Action<HelmetInstance> onHelmetInstanceDataChanged;
    public Action<HelmetInstance> onWearHelmetChanged;
    public Action onHelmetDamaged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("HelmetManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }

        CreateAllInstances();
    }

    private void Start()
    {
        Debug.Log("HelmetManager START");
        InitializeOwnedHelmets();
    }


    private void InitializeOwnedHelmets()
    {
        allHelmets[5].Discover();
        allHelmets[6].Discover();
        allHelmets[7].Discover();


        allHelmets[5].Craft();
        allHelmets[6].Craft();
        allHelmets[7].Craft();


        EquipHelmet(allHelmets[5]);
        EquipHelmet(allHelmets[6]);
        EquipHelmet(allHelmets[7]);

    }

    // Crear todas las instancias de cascos
    public void CreateAllInstances()
    {
        foreach( var _data in allHelmetData)
        {
            CreateHelmetInstance(_data);
        }
    }

    // Función para crear un helmet instance
    public void CreateHelmetInstance(HelmetData _helmet)
    {
        HelmetInstance _current = new HelmetInstance(_helmet);
        allHelmets.Add(_current);
    }

    public void Discover(HelmetData _data)
    {
        GetInstanceFromData(_data).Discover();
    }

    public HelmetInstance GetInstanceFromData(HelmetData _data)
    {
        foreach(var _instance in allHelmets)
        {
            if(_instance.baseHelmet == _data)
            {
                return _instance;
            }
        }

        return null;
    }


    // Función para EQUIPAR un casco, esto quiere decir que cargara con el casco durante la partida
    public void EquipHelmet()
    {
        HelmetInstance _craftedHelmet = CraftingManager.Instance.selectedHelmet;

        if (helmetsEquipped.Count < maxEquippedHelmets)
        {
            EquipHelmet(_craftedHelmet);

        } else
        {
            UIManager.Instance.craftingPanel.ToggleSwapPanel(true);
        }

    }

    public void EquipHelmet(HelmetInstance _craftedHelmet)
    {
        helmetsEquipped.Add(_craftedHelmet);
        onHelmetEquipped?.Invoke(_craftedHelmet);
        PlayerManager.Instance.AddMaxLives(1);
        WearHelmet(_craftedHelmet);
    }

    public void SwapHelmet(HelmetInstance _helmetIn, HelmetInstance _helmetOut)
    {
        var index = helmetsEquipped.FindIndex((h => h == _helmetOut));
        helmetsEquipped[index] = _helmetIn;
        onHelmetsSwapped?.Invoke(index);
        WearHelmet(_helmetIn);
    }

    // Función para USAR un casco 
    public void WearHelmet(HelmetInstance _helmet) {
        helmetIndex = helmetsEquipped.IndexOf(_helmet);
        currentHelmet = _helmet;
        currentMesh.SetHelmetMesh(_helmet.baseHelmet.mesh);
        onWearHelmetChanged?.Invoke(_helmet);
        _helmet.OnWear();
    }

    //Reseta los stats de los cascos equipados
    public void ResetHelmetsStats()
    {
        foreach (HelmetInstance _helmet in helmetsEquipped)
        {
            _helmet.ResetStats();
        }

        UIManager.Instance.currentHelmetsHUD.RefreshAllHelmets();
    }

    public void UseHelmetPotion(int _potionID)
    {
        currentHelmet.HealDurability(potionValues[_potionID]);
    }

    /* Funciones para cambiar entre cascos */

    //Función para cambiar al siguiente casco
    public void NextHelmet(InputAction.CallbackContext context)
    {
        //Si el jugador solo cuenta con 1 casco
        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log("+");
            if (helmetsEquipped.Count <= 1) return;

            WearNextAvailableHelmet();
        }


    }

    //Función para cambiar al casco anterior
    public void PreviousHelmet(InputAction.CallbackContext context)
    {
        //Si el jugador solo cuenta con 1 casco
        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log("-");
            if (helmetsEquipped.Count <= 1) return;

            WearPrevAvailableHelmet();
        }
    }

    public void WearNextAvailableHelmet()
    {
        int _ogIndex = helmetIndex;

        do
        {
            NextIndex();
        } while (helmetsEquipped[helmetIndex].IsWornOut & helmetIndex != _ogIndex);

        if (helmetIndex == _ogIndex) return;

        //MANUAL ENABLE A SUSCRIPCIONES
        foreach (HelmetEffect _effect in currentHelmet.activeEffects)
        {
            _effect.OnUnwear();
        }
        WearHelmet(helmetsEquipped[helmetIndex]);

        //Update UI

    }

    public void WearPrevAvailableHelmet()
    {
        int _ogIndex = helmetIndex;

        do
        {
            PreviousIndex();
        } while (helmetsEquipped[helmetIndex].IsWornOut & helmetIndex != _ogIndex);

        if (helmetIndex == _ogIndex) return;

        //MANUAL DISABLE A SUSCRIPCIONES
        foreach (HelmetEffect _effect in currentHelmet.activeEffects)
        {
            _effect.OnUnwear();
        }

        WearHelmet(helmetsEquipped[helmetIndex]);

        //Update UI

    }

    public void NextIndex()
    {
        helmetIndex = (helmetIndex + 1) % helmetsEquipped.Count;
    }

    public void PreviousIndex()
    {
        helmetIndex = (helmetIndex - 1 + helmetsEquipped.Count) % helmetsEquipped.Count;
    }

}
