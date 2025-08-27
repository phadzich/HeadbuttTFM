using System;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class LevelDoorSetup : MonoBehaviour, IBlockSetup
{

    public void SetupVariant(string _variant, MapContext _context)
    {
        int _levelIndex = int.Parse(_variant);
        GetComponent<LevelDoorBehaviour>().SetupBlock(_context, _levelIndex);
        // Registrar esta puerta como la salida
        LevelManager.Instance.currentExitDoor = gameObject;
    }
}