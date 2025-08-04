using UnityEngine;

[RequireComponent(typeof(CollectibleTrigger))]
public class BPCollectible : MonoBehaviour, ICollectibleEffect
{
    private Sublevel parentSublevel;
    private HelmetData helmetData;

    public void SetupBlock(string _variant, MapContext _context)
    {
        parentSublevel = _context.sublevel;
        helmetData = _context.sublevel.helmetToDiscover;

        if (isAlreadyDiscovered())
        {
            DeactivateBP();
        }
    }

    private bool isAlreadyDiscovered()
    {
        return HelmetManager.Instance.GetInstanceFromData(helmetData).isDiscovered;
    }

    public void Activate()
    {
        Debug.Log("BP OBTAINED!!!");
        LevelManager.Instance.currentSublevel.CollectBP();
        DeactivateBP();
    }

    private void DeactivateBP()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }
}
