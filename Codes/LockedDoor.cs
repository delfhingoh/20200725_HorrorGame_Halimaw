using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * LockedDoor script: This is a script to handle locked doors (TO EXIT STAGE) as the behaviours and
 * interaction should be the same.
 */
public class LockedDoor : MonoBehaviour, ICanBeInteracted<GameObject>
{
    [Header("")]
    [Header("LOCKED DOOR")]
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject thisUIScreen;
    [SerializeField] private Text feedbackText;

    private GameObject gameManager;
    private timer thisTime;

    private void Update()
    {
        if(key.GetComponent<PickMeUp>().GetIsThisInPlayerHand())
        {
            this.tag = "Unlocked";
        }
        else if(!key.GetComponent<PickMeUp>().GetIsThisInPlayerHand())
        {
            this.tag = "Locked";
        }

        if (!gameManager)
        {
            gameManager = GameObject.Find("GameManager");
            thisTime = gameManager.GetComponent<timer>();
        }
    }

    public void MouseClickInteraction()
    {
        if(this.tag == "Unlocked")
        {
            if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage1")
            {
                feedbackText.text = "Exit stage 1?";
                thisUIScreen.SetActive(true);
            }
            if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage2")
            {
                feedbackText.text = "Exit stage 2?";
                thisUIScreen.SetActive(true);
            }
            if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage3")
            {
                gameManager.GetComponent<ResultMenu>().ShowWinUIScreen();
                Time.timeScale = 0f;
            }

            DoNotUnload.doNotUnload.UnlockMouseCursor();
            DoNotUnload.doNotUnload.LockPlayerMovement();
        }
    }

    public void YesButton()
    {
        feedbackText.text = "";

        DoNotUnload.doNotUnload.LockMouseCursor();
        DoNotUnload.doNotUnload.UnlockPlayerMovement();

        thisTime.SetTimeLeftForPrevStage(thisTime.secsToFinish);

        if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage1")
        {
            ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("Stage2");
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage1");
        }
        else if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage2")
        {
            ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("Stage3");
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage2");
        }
    }

    public void NoButton()
    {
        thisUIScreen.SetActive(false);
        feedbackText.text = "";

        DoNotUnload.doNotUnload.LockMouseCursor();
        DoNotUnload.doNotUnload.UnlockPlayerMovement();
    }

    // This door is not pickable.
    public void PickableInteraction(GameObject _whichHand)
    {
    }
}
