using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * TutorialScript: This script handles the tutorial stages.
 */
public class TutorialScript : MonoBehaviour
{
    private enum TutorialState
    {
        NONE,
        WASD,
        SHIFT,
        JUMP,
        EXPLORE,
        DONE
    }

    [Header("")]
    [Header("TUTORIAL")]
    [SerializeField] private TutorialState currentTutorialState;
    [SerializeField] private GameObject thisUIScreen;
    [SerializeField] private GameObject firstTipText;
    [SerializeField] private GameObject secondTipText;

    [SerializeField] private Text textArea;
    [SerializeField] private string fullText;
    [SerializeField] private float textDelay;

    private string currentText;
    private bool canMoveOn;
    private bool isFinish;

    private void Start()
    {
        if (textDelay == 0f)
            textDelay = 0.1f;

        currentText = "";
        secondTipText = GameObject.Find("SecondTipText");
        firstTipText = GameObject.Find("FirstTipText");

        canMoveOn = false;
        isFinish = false;
    }

    private void Update()
    {
        if(!secondTipText || !firstTipText)
        {
            secondTipText = GameObject.Find("SecondTipText");
            firstTipText = GameObject.Find("FirstTipText");
        }

        if (thisUIScreen.activeInHierarchy)
        {
            TutorialStages();
        }
    }

    private void TutorialStages()
    {
        if(currentTutorialState == TutorialState.NONE)
        {
            MoveOntoNextState();
            ChangeText();
        }

        if(DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonInteraction>().GetIsAllMovementKeysPressed() &&
            currentTutorialState == TutorialState.WASD)
        {
            Debug.Log("WASD PRESSED");
            MoveOntoNextState();
            ChangeText();
        }
        if (DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonInteraction>().GetIsShiftPressed() &&
            currentTutorialState == TutorialState.SHIFT)
        {
            Debug.Log("SHIFT PRESSED");
            MoveOntoNextState();
            ChangeText();
        }
        if (DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonInteraction>().GetIsJumpPressed() &&
            currentTutorialState == TutorialState.JUMP)
        {
            MoveOntoNextState();
            ChangeText();
            Debug.Log("JUMP PRESSED");
        }
        if(currentTutorialState == TutorialState.EXPLORE)
        {
            StartCoroutine(WaitForThis());
        }

    }

    private void MoveOntoNextState()
    {
        if (currentTutorialState != TutorialState.DONE)
            currentTutorialState += 1;

        canMoveOn = false;
    }

    private void ChangeText()
    {
        switch (currentTutorialState)
        {
            case TutorialState.NONE:
                {
                    fullText = "";
                    break;
                }
            case TutorialState.WASD:
                {
                    fullText = "Press WASD to move.";
                    break;
                }
            case TutorialState.SHIFT:
                {
                    fullText = "Movement with SHIFT to run.";
                    break;
                }
            case TutorialState.JUMP:
                {
                    fullText = "Press SPACE to jump.";
                    break;
                }
            case TutorialState.EXPLORE:
                {
                    fullText = "Go on... Look around you...";
                    break;
                }
            case TutorialState.DONE:
                {
                    fullText = "";
                    isFinish = true;
                    break;
                }
        }

        ChangeTextOnScreen();
    }

    public void ChangeTextOnScreen()
    {
        if(currentText.Length > 0)
        {
            currentText = "";
            textArea.text = "";
        }

        currentText = fullText;
        textArea.text = currentText;
    }

    public IEnumerator WaitForThis()
    {
        yield return new WaitForSeconds(2f);

        this.gameObject.SetActive(false);

        StopAllCoroutines();
    }

    public string GetTutorialState()
    {
        return currentTutorialState.ToString();
    }

    public void FinishTutorial()
    {
        currentTutorialState = TutorialState.EXPLORE;
        ChangeText();
    }
}
