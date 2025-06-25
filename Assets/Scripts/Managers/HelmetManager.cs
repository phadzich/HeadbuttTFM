using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelmetManager : MonoBehaviour
{

    public static HelmetManager Instance;

    [Header("HELMETS")]
    public List<HelmetData> allHelmets;
    public List<HelmetInstance> helmetsOwned = new();
    public List<HelmetInstance> helmetsEquipped = new();

    [Header("Stats")]
    public int maxEquippedHelmets = 3;
    public int maxOwnHelmets = 10;
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
        InitializeOwnedHelmets();
    }

    //FUNCION DE PRUEBA PARA PROTOTIPO
    private void InitializeOwnedHelmets()
    {
        UnlockHelmet(allHelmets[0]);
        UnlockHelmet(allHelmets[1]);
        UnlockHelmet(allHelmets[2]);
        EquipHelmet(helmetsOwned[0]);
        EquipHelmet(helmetsOwned[1]);
        EquipHelmet(helmetsOwned[2]);


        WearHelmet(helmetsEquipped[helmetIndex]);
        PlayerManager.Instance.MaxUpLives();
    }

    // Función para desbloquear un casco, es decir que a partir de un blueprint se crea el casco
    public void UnlockHelmet(HelmetData _helmet)
    {
        if (helmetsOwned.Count < maxOwnHelmets)
        {
            HelmetInstance _current = new HelmetInstance(_helmet);
            helmetsOwned.Add(_current);
        }
        else
        {
            Debug.Log("No hay mas espacio para cascos en el armario");
        }
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


    // Obtiene los cascos que estan listos y pueden subir de nivel
    public List<HelmetInstance> GetHelmetsReadyToEvolve()
    {
        return helmetsOwned;
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
