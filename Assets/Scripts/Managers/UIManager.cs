using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // WIP, HAY QUE CREAR EVENTOS Y SUSCRIBIRNOS A ELLOS


    public EquippedHelmetsPanel equippedHelmetsPanel;

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

    private void Start()
    {
        //POR AHORA HARDCODED
        equippedHelmetsPanel.InstanceEquippedIndicators(HelmetManager.Instance.helmetsEquipped);
        equippedHelmetsPanel.UpdateWearingHelmet(0);

    }

    private void OnEnable()
    {
        HelmetManager.Instance.onHelmetsEquipped += OnHelmetsEquipped;
        HelmetManager.Instance.onHelmetInstanceDataChanged += OnHelmetInstanceDataChanged;
        HelmetManager.Instance.onWearHelmetChanged += OnWearHelmetChanged;
    }
    private void OnDisable()
    {
        HelmetManager.Instance.onHelmetsEquipped -= OnHelmetsEquipped;
        HelmetManager.Instance.onHelmetInstanceDataChanged -= OnHelmetInstanceDataChanged;
        HelmetManager.Instance.onWearHelmetChanged -= OnWearHelmetChanged;
    }
    private void OnHelmetsEquipped(List<HelmetInstance> _helmetList)
    {
        equippedHelmetsPanel.InstanceEquippedIndicators(_helmetList);
    }

    private void OnHelmetInstanceDataChanged(string _id)
    {
            equippedHelmetsPanel.UpdateHelmetInstanceInfo(_id);
    }

    private void OnWearHelmetChanged(int _index)
    {
        equippedHelmetsPanel.UpdateWearingHelmet(_index);
    }

}
