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
    public Dictionary<string, HelmetInstance> helmetsOwned = new Dictionary<string,HelmetInstance>();
    public List<HelmetInstance> helmetsEquipped = new();
    public int maxEquippedHelmets = 3;
    public bool HasHelmetsLeft => helmetsEquipped.Count(helmet => !helmet.isWornOut) >= 1;

    [Header("CURRENT HELMET")]
    public HelmetInstance currentHelmet;
    public HelmetMesh currentMesh;
    public int helmetIndex = 0;

    public Action<List<HelmetInstance>> onHelmetsEquipped;

    //public Action<HelmetInstance> onHelmetInstanceDataChanged;
    public Action<HelmetInstance> onWearHelmetChanged;

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
        InitializeOwnedHelmets();
    }

    //FUNCION DE PRUEBA PARA PROTOTIPO
    private void InitializeOwnedHelmets()
    {
        HelmetInstance helmet1 = new HelmetInstance(allHelmets[0]);
        HelmetInstance helmet2 = new HelmetInstance(allHelmets[1]);
        HelmetInstance helmet3 = new HelmetInstance(allHelmets[2]);
        UnlockHelmet(helmet1);
        UnlockHelmet(helmet2);
        UnlockHelmet(helmet3);
        EquipHelmet(helmet1);
        EquipHelmet(helmet2);
        EquipHelmet(helmet3);
        onHelmetsEquipped?.Invoke(helmetsEquipped);

        WearHelmet(helmetsEquipped[helmetIndex]);

    }

    public void UnlockHelmet(HelmetInstance helmet)
    {
        helmetsOwned.Add(helmet.id, helmet);
    }

    public HelmetInstance GetHelmetByID(string id)
    {
        return helmetsOwned[id];
    }

    public void EquipHelmet(HelmetInstance helmet)
    {
        if(helmetsEquipped.Count < maxEquippedHelmets)
        {
            helmetsEquipped.Add(helmet);
        } else
        {
            Debug.Log("No hay mas espacio para cascos");
        }
       
    }

    public void WearHelmet(HelmetInstance helmet) {
        currentHelmet = helmet;
        currentMesh.SetHelmetMesh(helmet.baseHelmet.mesh);
        onWearHelmetChanged?.Invoke(helmet);
    }

    public void ResetHelmetsStats()
    {
        foreach (HelmetInstance helmet in helmetsEquipped)
        {
            helmet.ResetStats();
        }
    }

    public void NextHelmet(InputAction.CallbackContext context)
    {
        //Si el jugador solo cuenta con 1 casco
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("+");
            if (helmetsEquipped.Count <= 1) return;

            WearNextAvailableHelmet();
        }


    }

    public void PreviousHelmet(InputAction.CallbackContext context)
    {
        //Si el jugador solo cuenta con 1 casco
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("-");
            if (helmetsEquipped.Count <= 1) return;

            WearPrevAvailableHelmet();
        }
    }

    public void WearNextAvailableHelmet()
    {
        int ogIndex = helmetIndex;

        do
        {
            NextIndex();
        } while (helmetsEquipped[helmetIndex].isWornOut & helmetIndex != ogIndex);

        if (helmetIndex == ogIndex) return;
       

        WearHelmet(helmetsEquipped[helmetIndex]);

        //Update UI

    }

    public void WearPrevAvailableHelmet()
    {
        int ogIndex = helmetIndex;

        do
        {
            PreviousIndex();
        } while (helmetsEquipped[helmetIndex].isWornOut & helmetIndex != ogIndex);

        if (helmetIndex == ogIndex) return;

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
