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

        // PRUEBA SOLO PARA TENER HELMETS
        //InitializeOwnedHelmets();

    }

    private void Start()
    {
        Debug.Log("HelmetManager START");

        CreateAllInstances();
    }

    /*FUNCION DE PRUEBA PARA PROTOTIPO
    private void InitializeOwnedHelmets()
    {
        UnlockHelmet(allHelmets[0]);
        UnlockHelmet(allHelmets[1]);
        UnlockHelmet(allHelmets[2]);
        EquipHelmet(helmetsOwned[0]);
        EquipHelmet(helmetsOwned[1]);
        EquipHelmet(helmetsOwned[2]);

        //EVOLVE EL FIRE A LVL 2 PARA PRUEBAS
        helmetsOwned[1].Evolve(helmetsOwned[1].GetUpgradeRequirement(2));
        helmetsOwned[2].Evolve(helmetsOwned[2].GetUpgradeRequirement(2));

        WearHelmet(helmetsEquipped[helmetIndex]);
        PlayerManager.Instance.MaxUpLives();
    }*/

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
    public void EquipHelmet(HelmetInstance _helmet)
    {
        if (helmetsEquipped.Count < maxEquippedHelmets)
        {
            helmetsEquipped.Add(_helmet);
            onHelmetEquipped?.Invoke(_helmet);
            PlayerManager.Instance.AddMaxLives(1);
        } else
        {
            Debug.Log("No hay mas espacio para cascos");
        }

    }

    // Función para USAR un casco 
    public void WearHelmet(HelmetInstance _helmet) {
        currentHelmet = _helmet;
        currentMesh.SetHelmetMesh(_helmet.currentInfo.mesh);
        onWearHelmetChanged?.Invoke(_helmet);
        foreach(HelmetEffect _effect in _helmet.activeEffects)
        {
            _effect.OnWear();
        }
    }



    public void TryUseHelmetSpecial(InputAction.CallbackContext context)
    {
        //SI NO HAY HELMET
        if (currentHelmet == null) return;

        //CUANDO EL INPUT ESTA PERFORMED
        if (context.phase == InputActionPhase.Performed)
            {
            //BUSCA TODOS LOS EFECTOS CON SPECIAL ATTACKS Y LOS ACTIVA
            foreach (HelmetEffect _effect in currentHelmet.activeEffects)
            {
                if (_effect.hasSpecialAttack)
                {
                    //BASTA QUE UNO EFFECT TENGA SPECIAL, LLAMAMOS A TODOS
                    currentHelmet.OnSpecialAttack();
                    break;
                }
            }

        }
    }

    //Reseta los stats de los cascos equipados
    public void ResetHelmetsStats()
    {
        foreach (HelmetInstance _helmet in helmetsEquipped)
        {
            _helmet.ResetStats();
        }
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
