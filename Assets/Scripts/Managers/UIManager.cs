using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // WIP, HAY QUE CREAR EVENTOS Y SUSCRIBIRNOS A ELLOS


    public EquippedHelmetsPanel equippedHelmetsPanel;
    public HeadbuttsPanel headbuttsPanel;

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
        headbuttsPanel.UpdateUsedHeadbutts(HelmetManager.Instance.helmetsEquipped[0]);

        equippedHelmetsPanel.InstanceEquippedIndicators(HelmetManager.Instance.helmetsEquipped);
        equippedHelmetsPanel.UpdateWearingHelmet(HelmetManager.Instance.helmetsEquipped[0]);


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

    private void OnHelmetInstanceDataChanged(HelmetInstance _instance)
    {
        //Debug.Log("INSTANCE DATA CHANGED"+ _instance.id);
            equippedHelmetsPanel.UpdateHelmetInstanceInfo(_instance);
        if (_instance.maxHeadbutts>0)
        {
            headbuttsPanel.UpdateUsedHeadbutts(_instance);
        }

    }

    private void OnWearHelmetChanged(HelmetInstance _instance)
    {
        equippedHelmetsPanel.UpdateWearingHelmet(_instance);
        headbuttsPanel.UpdateUsedHeadbutts(_instance);

    }

}
