using System;
using UnityEngine;

[RequireComponent(typeof(BlockNS))]
public class DoorSetup : MonoBehaviour, IBlockSetup
{

    public void SetupVariant(string _variant, MapContext _context)
    {
        if(int.Parse(_variant) == 1)
        {
            GetComponent<DoorBehaviour>().SetupBlock(_context);
        }
        else
        {
            GetComponent<NPCBehaviour>().SetupBlock(_context);
        }
        
        // Registrar esta puerta como la salida
        LevelManager.Instance.currentExitDoor = gameObject;
    }
}