using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CircuitBoard: This is to be attached on the main circuit switch
 */
public class MainCircuitSwitch : MonoBehaviour, ICanBeInteracted<GameObject>
{
    [SerializeField] private GameObject[] ceilingLightArray;

    private AnimationForThis thisAnimation;
    private Light thisLight;
    private bool isSwitchOn;

    private void Start()
    {
        thisAnimation = GetComponent<AnimationForThis>();
        isSwitchOn = false;

        foreach(GameObject thisOBJ in ceilingLightArray)
        {
            thisLight = thisOBJ.GetComponentInChildren<Light>();
            thisLight.intensity = 0f;
        }
    }

    public void MouseClickInteraction()
    { 
        if(!isSwitchOn)
        {
            thisAnimation.PlayAnimation("SwitchOn");
            isSwitchOn = true;

            foreach (GameObject thisOBJ in ceilingLightArray)
            {
                thisLight = thisOBJ.GetComponentInChildren<Light>();
                thisLight.intensity = 4f;
            }
        }
        else
        {
            thisAnimation.PlayAnimation("SwitchOff");
            isSwitchOn = false;

            foreach (GameObject thisOBJ in ceilingLightArray)
            {
                thisLight = thisOBJ.GetComponentInChildren<Light>();
                thisLight.intensity = 0f;
            }
        }
    }

    // This object is not pickable.
    public void PickableInteraction(GameObject _whichHand)
    {
    }
}
