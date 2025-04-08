using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // WIP, HAY QUE CREAR EVENTOS Y SUSCRIBIRNOS A ELLOS

    public static UIManager Instance;
    public ResourcesPanel resourcesPanel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("UIManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }

    }

}
