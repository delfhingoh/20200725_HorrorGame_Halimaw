using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PickMeUp: This is a script that should be attached to all objects that are
 * pickable by player. It is a generic script that handles the behaviour / interactions
 * with this picakable object.
 */
 [RequireComponent(typeof(Rigidbody))]
public class PickMeUp : MonoBehaviour, ICanBeInteracted<GameObject>
{
    private bool isPickUp;
    private bool isThisObjInPlayerHand;

    private void Start()
    {
        isPickUp = false;
        isThisObjInPlayerHand = false;
    }

    public void MouseClickInteraction()
    {
    }

    public void PickableInteraction(GameObject whichHand)
    {
        if (GetIsPickUp())
            PickUp(whichHand);

        else if (!GetIsPickUp())
            DropThis(whichHand);
    }

    public bool GetIsPickUp()
    {
        return isPickUp;
    }

    public void SetIsPickUp(bool _isPickUp)
    {
        isPickUp = _isPickUp;
    }

    public bool GetIsThisInPlayerHand()
    {
        return isThisObjInPlayerHand;
    }

    public void ResetPickMeUp()
    {
        isPickUp = false;
        isThisObjInPlayerHand = false;

        if (this.transform.parent)
            this.transform.parent = null;
    }

    private void PickUp(GameObject thisHand)
    {
        this.transform.parent = thisHand.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;

        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.GetComponent<Collider>().enabled = false;

        isThisObjInPlayerHand = true;
    }

    private void DropThis(GameObject thisHand)
    {
        this.transform.parent = null;

        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Collider>().enabled = true;

        isThisObjInPlayerHand = false;            
    }
}
