using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private float visionModifier = 1.5f;
    private int harvestModifier = 2;
    private int lavaDamage = 1;

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

    // Efectos de helmet

    // Efecto que hace harvest al doble
    public float GetHBHarvestModified(float _harvest)
    {
        return _harvest * harvestModifier;
    }

    //Efecto que modifica el rango de vision
    public float GetVisionModifier()
    {
        return visionModifier;
    }

    public int GetLavaDamage()
    {
        return lavaDamage;
    }
}

