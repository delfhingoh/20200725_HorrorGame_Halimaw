using UnityEngine;

/*
 * HidingSpot: This script is to be attached on any area that is for player to hide.
 * The hiding area must have a collider with isTrigger as true.
 */
public class HidingSpot : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FirstPersonManager>().SetIsPlayerHiding(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<FirstPersonManager>().SetIsPlayerHiding(false);
        }
    }
}
