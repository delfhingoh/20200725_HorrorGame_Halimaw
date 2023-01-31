using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    [SerializeField] private GameObject[] lightsArray;
    [SerializeField] private int lightIndex;
    [SerializeField] private float flickerTiming;
    private float previousIntenstiy;
    private bool canTurnOff;

    private Light thisLight;

    private void Start()
    {
        lightsArray = GameObject.FindGameObjectsWithTag("Light");
        lightIndex = Random.Range(0, lightsArray.Length - 1);
        flickerTiming = Random.Range(3.0f, 10.0f);

        thisLight = lightsArray[lightIndex].GetComponentInChildren<Light>();
        previousIntenstiy = thisLight.intensity;

        canTurnOff = false;
    }

    private void Update()
    {

        if (canTurnOff)
        {
            thisLight.intensity = 0f;
            StartCoroutine(TurnOn());
        }
        else
        {
            thisLight.intensity = previousIntenstiy;
            StartCoroutine(TurnOff());
        }
        
    }

    private IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(flickerTiming);
        canTurnOff = true;
    }

    private IEnumerator TurnOn()
    {
        yield return new WaitForSeconds(flickerTiming);
        canTurnOff = false;
    }
}
