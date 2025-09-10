using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IBlockBehaviour
{

    public void OnBounced(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
        MatchManager.Instance.FloorBounced();
    }

    public void StartBehaviour()
    {
    }

    public void StopBehaviour()
    {
    }

}
