using UnityEngine;
using System.Collections;
using System;

public class OnOffBehaviour : MonoBehaviour, IBlockBehaviour
{
    public GameObject prefabToSwitch;
    public float onTime;
    public float offTime;

    public bool randomStartDelay;
    public float randomStartDelayRange;



    public void StartBehaviour()
    {
        if (randomStartDelay)
        {
            float _delay = UnityEngine.Random.Range(0, randomStartDelayRange);
            StartCoroutine(DelayTimer(_delay));
        }
        else
        {
            StartCoroutine(DelayOn());
        }
    }

    public void StopBehaviour()
    {
        StopAllCoroutines();
    }

    private IEnumerator DelayTimer(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        StartCoroutine(DelayOn());
    }

    private IEnumerator DelayOn() { 
        yield return new WaitForSeconds(offTime);
        TurnOn();
    }

    private IEnumerator DelayOff()
    {
        yield return new WaitForSeconds(onTime);
        TurnOff();
    }

    private void TurnOn()
    {
        prefabToSwitch.SetActive(true);
        StartCoroutine(DelayOff());
    }

    private void TurnOff()
    {
        prefabToSwitch.SetActive(false);
        StartCoroutine(DelayOn());
    }

    public void OnBounced(HelmetInstance _helmetInstance)
    {
    }

    public void OnHeadbutt(HelmetInstance _helmetInstance)
    {
    }
}
