using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * ThisIsLocked: This script is for doors that are locked in game world and NOT exiting scene.
 * For now, this is written for store room door.
 */
public class ThisIsLocked : MonoBehaviour, ICanBeInteracted<GameObject>
{
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject thisLockedOBJ;
    [SerializeField] private Material unlockedMat;

    private Renderer thisRenderer;
    private Material[] thisMats;
    private AnimationForThis animForThis;

    private bool isDone;

    private void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        thisMats = thisRenderer.materials;

        animForThis = thisLockedOBJ.GetComponent<AnimationForThis>();

        isDone = false;
    }

    private void Update()
    {
        if(SaveManager.saveManager.isSaveFileLoaded)
        {
            if (this.tag == "Unlocked" && !isDone)
            {
                thisMats[1] = unlockedMat;
                thisRenderer.materials = thisMats;

                animForThis.PlayAnimation("Open");

                isDone = true;
            }
        }
    }

    public void MouseClickInteraction()
    {
        if (key.GetComponent<PickMeUp>().GetIsThisInPlayerHand())
            this.tag = "Unlocked";
        else if (!key.GetComponent<PickMeUp>().GetIsThisInPlayerHand())
            this.tag = "Locked";

        // If Player has the KEY
        if (this.tag == "Unlocked")
        {
            thisMats[1] = unlockedMat;
            thisRenderer.materials = thisMats;

            animForThis.PlayAnimation("Open");

            // Remove the key from player's hand
            key.transform.parent = null;
            key.SetActive(false);
        }
    }

    // This LOCKED object is not PICKABLE
    public void PickableInteraction(GameObject _whichHand)
    {
    }
}