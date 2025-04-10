using System.Collections.Generic;
using UnityEngine;

public class HelmetManager : MonoBehaviour
{

    public static HelmetManager Instance;

    public List<HelmetData> allHelmets;
    public List<HelmetData> helmetsOwned;
    public List<HelmetInstance> helmetsEquipped;

    public HelmetInstance currentHelmet;

    [SerializeField]
    public Dictionary<ResourceData, int> ownedResources = new();

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
    }

    private void Start()
    {

        // PRUEBA SOLO PARA TENER HELMETS
        HelmetInstance helmet1 = new HelmetInstance(helmetsOwned[0]);
        HelmetInstance helmet2 = new HelmetInstance(helmetsOwned[1]);

        helmetsEquipped.Add(helmet1);
        helmetsEquipped.Add(helmet2);

        EquipHelmet(helmetsEquipped[0]);
    }

    public void EquipHelmet(HelmetInstance helmet) {
        currentHelmet = helmet;
    }

}
