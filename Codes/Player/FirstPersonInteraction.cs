using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * FirstPersonInteraction: This script handles all interactions made by the player.
 */
public class FirstPersonInteraction : MonoBehaviour
{
    [SerializeField] private float distBtwnPlayer;
    [SerializeField] private Transform rightGrabArea;
    [SerializeField] private Transform leftGrabArea;
    [SerializeField] private TutorialScript tutorialScript;

    private InputTipScript firstTipText;
    private InputTipScript secondTipText;

    private Camera mainCamera;
    private RaycastHit hit;
    private PickMeUp pickMeUp;

    private GameObject hitObject;
    private GameObject currentRightHandHeldOBJ;
    private GameObject currentLeftHandHeldOBJ;

    private float pickUpCoolDown;
    private bool isThisMouseInteraction;

    private bool isRightHandFull;
    private bool isLeftHandFull;
    private bool isRightHandReady;
    private bool isLeftHandReady;

    private bool isWPressed;
    private bool isAPressed;
    private bool isSPressed;
    private bool isDPressed;
    private bool isShiftPressed;
    private bool isJumpPressed;

    private void Start()
    {
        if (distBtwnPlayer == 0)
            distBtwnPlayer = 4f;

        if (!rightGrabArea)
            rightGrabArea = GameObject.Find("RightGrabArea").transform;
        if (!leftGrabArea)
            leftGrabArea = GameObject.Find("LeftGrabArea").transform;

        firstTipText = GameObject.Find("FirstTipText").GetComponent<InputTipScript>();
        secondTipText = GameObject.Find("SecondTipText").GetComponent<InputTipScript>();

        mainCamera = Camera.main;
        pickMeUp = null;

        hitObject = null;

        pickUpCoolDown = 0.1f;
        isThisMouseInteraction = false;

        isRightHandFull = false;
        isLeftHandFull = false;
        isRightHandReady = true;
        isLeftHandReady = true;
    }

    private void Update()
    {
        if(!mainCamera)
            mainCamera = Camera.main;

        // Tutorial
        if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage1")
        {
            if (SaveManager.saveManager.isSaveFileLoaded)
            {
                tutorialScript.FinishTutorial();
            }

            if (tutorialScript.gameObject.activeInHierarchy)
            {
                if (tutorialScript.GetTutorialState() == "WASD")
                {
                    if (Input.GetKeyDown(KeyCode.W))
                        isWPressed = true;
                    if (Input.GetKeyDown(KeyCode.A))
                        isAPressed = true;
                    if (Input.GetKeyDown(KeyCode.S))
                        isSPressed = true;
                    if (Input.GetKeyDown(KeyCode.D))
                        isDPressed = true;
                }
                if (tutorialScript.GetTutorialState() == "SHIFT")
                    if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                        isShiftPressed = true;
                if (tutorialScript.GetTutorialState() == "JUMP")
                    if (Input.GetKeyDown(KeyCode.Space))
                        isJumpPressed = true;
            }
        }

        // Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            DoNotUnload.doNotUnload.gameManagerOBJ.GetComponent<PauseMenu>().ShowUIScreen();

            secondTipText.ClearTipText();
        }

        if (isRightHandFull)
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!currentRightHandHeldOBJ)
                    currentRightHandHeldOBJ = rightGrabArea.transform.GetChild(0).gameObject;

                currentRightHandHeldOBJ.GetComponent<PickMeUp>().SetIsPickUp(false);
                currentRightHandHeldOBJ.GetComponent<ICanBeInteracted<GameObject>>().PickableInteraction(rightGrabArea.gameObject);
                currentRightHandHeldOBJ = null;

                StartCoroutine(WaitForHand(rightGrabArea.gameObject));
            }
        if (isLeftHandFull)
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!currentLeftHandHeldOBJ)
                    currentLeftHandHeldOBJ = leftGrabArea.transform.GetChild(0).gameObject;

                currentLeftHandHeldOBJ.GetComponent<PickMeUp>().SetIsPickUp(false);
                currentLeftHandHeldOBJ.GetComponent<ICanBeInteracted<GameObject>>().PickableInteraction(leftGrabArea.gameObject);
                currentLeftHandHeldOBJ = null;

                StartCoroutine(WaitForHand(leftGrabArea.gameObject));
            }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(GetScreenPointToRay(), out hit, distBtwnPlayer, (1 << LayerMask.NameToLayer("Interactable"))))
            {
                if(EventSystem.current.firstSelectedGameObject)
                {
                    EventSystem.current.firstSelectedGameObject.GetComponent<ObjectOutlineScript>().SetIsOutlineOn(false);
                    EventSystem.current.firstSelectedGameObject = null;
                }

                hitObject = hit.transform.gameObject;
                hitObject.GetComponent<ObjectOutlineScript>().SetIsOutlineOn(true);
                EventSystem.current.firstSelectedGameObject = hitObject;

                TipText(hitObject);

                // Mouse Click
                if (Input.GetMouseButtonDown(0))
                {
                    hitObject.GetComponent<ICanBeInteracted<GameObject>>().MouseClickInteraction();
                }

                // Pickable Items. Press [E] or [R] on keyboard.
                if (hitObject.tag == "Pickable")
                {
                    if (!isRightHandFull && isRightHandReady)
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            currentRightHandHeldOBJ = hitObject;
                            currentRightHandHeldOBJ.GetComponent<PickMeUp>().SetIsPickUp(true);
                            currentRightHandHeldOBJ.GetComponent<ICanBeInteracted<GameObject>>().PickableInteraction(rightGrabArea.gameObject);

                            isRightHandFull = true;
                            isRightHandReady = false;
                        }
                    if (!isLeftHandFull && isLeftHandReady)
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            currentLeftHandHeldOBJ = hitObject;
                            currentLeftHandHeldOBJ.GetComponent<PickMeUp>().SetIsPickUp(true);
                            currentLeftHandHeldOBJ.GetComponent<ICanBeInteracted<GameObject>>().PickableInteraction(leftGrabArea.gameObject);

                            isLeftHandFull = true;
                            isLeftHandReady = false;
                        }
                }
            }
            else
            {
                if (EventSystem.current.firstSelectedGameObject)
                {
                    EventSystem.current.firstSelectedGameObject.GetComponent<ObjectOutlineScript>().SetIsOutlineOn(false);
                    EventSystem.current.firstSelectedGameObject = null;
                }

                firstTipText.ClearTipText();
                secondTipText.ClearTipText();
            }
        }
    }

    private void TipText(GameObject thisOBJ)
    {
        // Check if this OBJ is pickable or mouse interaction
        if (thisOBJ.GetComponent<PickMeUp>())
        {
            isThisMouseInteraction = false;
            pickMeUp = thisOBJ.GetComponent<PickMeUp>();
        }
        else
            isThisMouseInteraction = true;

        // Set Tip Text
        if (thisOBJ.tag == "Locked" && isThisMouseInteraction)
            firstTipText.SetTipText("This " + thisOBJ.name + " is locked.");
        else if (isThisMouseInteraction)
            firstTipText.SetTipText("Left mouse click on " + thisOBJ.name);
        if (!isThisMouseInteraction)
            firstTipText.SetTipText("[E] for left hand or [R] for right hand to pick " + thisOBJ.name + " up.");
    }

    private Ray GetScreenPointToRay()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        return ray;
    }

    private IEnumerator WaitForHand(GameObject thisHand)
    {
        yield return new WaitForSeconds(pickUpCoolDown);

        Debug.Log("Hand is ready.");

        if (thisHand.name == "RightGrabArea")
        {
            isRightHandReady = true;
            isRightHandFull = false;
        }
        else if (thisHand.name == "LeftGrabArea")
        {
            isLeftHandReady = true;
            isLeftHandFull = false;
        }

        StopCoroutine(WaitForHand(thisHand));
    }

    public bool GetIsAllMovementKeysPressed()
    {
        if (isWPressed && isAPressed && isSPressed && isDPressed)
            return true;

        return false;
    }

    public bool GetIsShiftPressed()
    {
        if (isShiftPressed)
            return true;

        return false;
    }

    public bool GetIsJumpPressed()
    {
        if (isJumpPressed)
            return true;

        return false;
    }


    public bool GetIsRightHandFull()
    {
        return isRightHandFull;
    }

    public bool GetIsLeftHandFull()
    {
        return isLeftHandFull;
    }

    public void SetIsRightHandFull(bool _isFull)
    {
        isRightHandFull = _isFull;
    }

    public void SetIsLeftHandFull(bool _isFull)
    {
        isLeftHandFull = _isFull;
    }
}
