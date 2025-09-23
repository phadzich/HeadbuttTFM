using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SwitchSetup))]
public class SwitchBehaviour : MonoBehaviour, IBlockBehaviour
{
    public MapContext mapContext;
    [SerializeField] public float duration;
    [SerializeField] public float elapsedTime;
    public bool isActive;
    public GameObject buttonMesh;
    public GameObject shapeMesh;
    public Sublevel parentSublevel;
    public MeshFilter meshFilter; 
    public MeshRenderer meshRenderer;
    public List<Material> materialList;
    public List<Sprite> iconsList;
    public List<Mesh> shapeMeshes;
    [SerializeField]private int switchID;
    public GameObject timerUI;
    public Image timerFill;
    public TextMeshProUGUI timerText;
    public Sprite switchIcon;

    private void Update()
    {
        if (isActive)
        {
            elapsedTime += Time.deltaTime;

            timerText.text = ((int)(duration-elapsedTime)).ToString();
            timerFill.fillAmount = 1-(elapsedTime/duration);

            if (elapsedTime > duration)
            {

                DeactivateSwitch();
            }
        }
    }

    private void ActivateSwitch()
    {

        ToggleShapeMesh(true);
        isActive = true;
        elapsedTime = 0;
        DispatchStateEvent(isActive);
        timerUI.SetActive(true);
    }

    private void DeactivateSwitch()
    {

        CombatLogHUD.Instance.AddLog(switchIcon, $"<b>SWITCH</b> expired!");

        ToggleShapeMesh(false);
        isActive = false;
        DispatchStateEvent(isActive);
        timerUI.SetActive(false);

    }

    private void DispatchStateEvent(bool _condition)
    {
        var _switchEvent = new ActiveSwitchEvent();
        _switchEvent.isActive = _condition;
        _switchEvent.switchID = switchID;
        _switchEvent.switchBehaviour = this;
        LevelManager.Instance.currentSublevel.DispatchObjectiveEvent(_switchEvent);
    }

    public void SetupBlock(MapContext _context, int _id)
    {
        GetComponent<BlockNS>().isWalkable = true;
        mapContext = _context;
        switchID = _id;
        ChangeMeshByID();
        ToggleShapeMesh(false);
        GetDurationFromSublevel();
        timerUI.SetActive(false);
    }

    private void ToggleShapeMesh(bool _value)
    {
        shapeMesh.SetActive(_value);
    }

    private void ChangeMeshByID()
    {
        meshFilter.mesh = shapeMeshes[switchID];
        meshRenderer.material = materialList[switchID];
        switchIcon = iconsList[switchID];
    }

    private void GetDurationFromSublevel()
    {
        for (int i = 0; i < mapContext.sublevel.activeSwitchesDurations.Count; i++)
        {
            if (i == switchID)
            {
                duration = mapContext.sublevel.activeSwitchesDurations[i];
                break;
            }
        }
    }
    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        if (!isActive) ActivateSwitch();
        MatchManager.Instance.FloorBounced();
    }

    public void Activate()
    {
        
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }
}
