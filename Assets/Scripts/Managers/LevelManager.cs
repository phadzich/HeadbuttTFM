using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject resourceBlockPrefab;
    public Transform sublevelContainer;

    public List<ResourceBlock> resourceBlocks;

    public int sublevelWidth;
    public int sublevelHeight;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("LevelManager Awake");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateSublevel(sublevelWidth, sublevelHeight);
    }

    public void GenerateSublevel(int _cols, int _rows)
    {
        Debug.Log("Generating Sublevel");
        DestroyAllBlocks();
        InstanceNewBlocks(_cols, _rows);
    }

    public void InstanceNewBlocks(int  _cols, int _rows)
    {
        int _spacing = 1;

        float offsetX = (_cols - 1) * _spacing * 0.5f;
        float offsetZ = (_cols - 1) * _spacing * 0.5f;

        for (int z = 0; z < _rows; z++)
        {
            for (int x = 0; x < _cols; x++)
            {
                Vector3 _posicion = new Vector3(x * _spacing - offsetX, 0, z * _spacing - offsetZ);
                GameObject _bloque = Instantiate(resourceBlockPrefab, _posicion, Quaternion.identity, sublevelContainer);
                ResourceBlock _resourceBlock = _bloque.GetComponent<ResourceBlock>();
                _resourceBlock.SetupBlock(0, x, x, ResourceManager.Instance.allAvailableResources[Random.Range(0, 3)]);
                _bloque.name = $"{_resourceBlock.resourceData.shortName}_c{x}r_{z}";
                resourceBlocks.Add(_resourceBlock);
                if (x == _cols/2 && z == _rows/2)
                {
                    _resourceBlock.isDoor = true;
                }
            }
        }
    }

    private void DestroyAllBlocks()
    {
        foreach (Transform child in sublevelContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
