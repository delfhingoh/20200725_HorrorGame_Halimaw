using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Note: This script currently handles all notes.
 */
public class Note : MonoBehaviour, ICanBeInteracted<GameObject>
{
    [SerializeField] private GameObject thisUIScreen;
    [SerializeField] private Text thisText;
    [SerializeField] private string words;

    public void MouseClickInteraction()
    {
        thisText.text = words;
        ShowNoteScreen();
    }

    // This is a non-pickable item
    public void PickableInteraction(GameObject whichHand)
    {
    }

    private void ShowNoteScreen()
    {
        DoNotUnload.doNotUnload.UnlockMouseCursor();
        DoNotUnload.doNotUnload.LockPlayerMovement();

        thisUIScreen.SetActive(true);
    }

    public void CloseNoteScreen()
    {
        DoNotUnload.doNotUnload.LockMouseCursor();
        DoNotUnload.doNotUnload.UnlockPlayerMovement();

        thisUIScreen.SetActive(false);
    }
}
