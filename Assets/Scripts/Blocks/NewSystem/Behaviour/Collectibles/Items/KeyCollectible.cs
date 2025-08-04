using UnityEngine;

[RequireComponent(typeof(CollectibleTrigger))]
public class KeyCollectible : MonoBehaviour, ICollectibleEffect
{
    public void Activate()
    {
        Debug.Log("KEY OBTAINED!!!");
        LevelManager.Instance.currentSublevel.CollectKey(1);
    }

    public void SetupBlock(string _variant, MapContext _context)
    {
        
    }
}
