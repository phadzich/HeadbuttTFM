using PrimeTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelDoorSetup))]
public class LevelDoorBehaviour : MonoBehaviour, IBlockBehaviour
{
    public MapContext mapContext;
    public int targetLevelIndex; 
    public EnterLevelUI enterLevelUI;
      public void SetupBlock(MapContext _context, int _targetLevelIndex)
    {
        mapContext = _context;
        targetLevelIndex = _targetLevelIndex;
        enterLevelUI.Setup(LevelManager.Instance.levelsList[_targetLevelIndex]);

    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        
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
