using System;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
[RequireComponent(typeof(DoorBehaviour))]
public class DoorSetup : MonoBehaviour, IBlockSetup
{

    public void SetupVariant(string _variant, MapContext _context)
    {
        GetComponent<DoorBehaviour>().SetupBlock(_context);

        // Registrar esta puerta como la salida
        LevelManager.Instance.currentExitDoor = gameObject;
    }
}