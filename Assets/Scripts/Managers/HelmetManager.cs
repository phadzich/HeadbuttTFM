using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelmetManager : MonoBehaviour
{

    public static HelmetManager Instance;

    [Header("HELMETS")]
    public List<HelmetData> allHelmets;
    public List<HelmetData> helmetsOwned;
    public List<HelmetInstance> helmetsEquipped;
    public int maxEquippedHelmets = 3;
    public bool HasHelmetsLeft => helmetsEquipped.Count(helmet => !helmet.isWornOut) >= 1;

    [Header("CURRENT HELMET")]
    public HelmetInstance currentHelmet;
    public HelmetMesh currentMesh;
    public int helmetIndex = 0;
    

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
        HelmetInstance helmet1 = new HelmetInstance(helmetsOwned[0]);
        HelmetInstance helmet2 = new HelmetInstance(helmetsOwned[1]);

        EquipHelmet(helmet1);
        EquipHelmet(helmet2);

        WearHelmet(helmetsEquipped[helmetIndex]);
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
        if (helmetsEquipped.Count <= 1) return;

        EquipNextAvailableHelmet();
    }

    public void PreviousHelmet(InputAction.CallbackContext context)
    {
        //Si el jugador solo cuenta con 1 casco
        if (helmetsEquipped.Count <= 1) return;

        EquipPrevAvailableHelmet();
    }

    public void EquipNextAvailableHelmet()
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

    public void EquipPrevAvailableHelmet()
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
