using System.Collections;
using UnityEngine;

/*
 * DoNotUnload: This script handles all the GAMEOBJECTS that MUST stay
 * in the DoNotUnload Scene in replacement of DONTDESTROYONLOAD.
 * 
 * HOWEVER there is a KNOWN ISSUE when it comes to finding INACTIVE OBJECTS
 * therefore multiple public objects are here. There is also the issue of objects missing after transisting 
 * to different scenes but this seems to fix it. ( I HOPE )
 */
public class DoNotUnload : MonoBehaviour
{
    public static DoNotUnload doNotUnload;

    public GameObject FPController;
    public GameObject mouseCursorOBJ;
    public GameObject saveManagerOBJ;
    public GameObject gameManagerOBJ;

    private string _sceneName;
    private bool isThisMainMenu;

    public void DoNotUnloadStart()
    {
        StartCoroutine(Begin());
    }

    public void LockMouseCursor()
    {
        FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.m_cursorIsLocked = true;
        FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.SetCursorLock(true);

        mouseCursorOBJ.SetActive(true);
    }

    public void UnlockMouseCursor()
    {
        FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.m_cursorIsLocked = false;
        FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.SetCursorLock(false);

        mouseCursorOBJ.SetActive(false);
    }

    public void LockPlayerMovement()
    {
        FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().canMove = false;
    }

    public void UnlockPlayerMovement()
    {
        FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().canMove = true;
    }

    private void Awake()
    {
        doNotUnload = this;
    }

    private IEnumerator Begin()
    {
        yield return new WaitForSeconds(0.1f);

        if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "MainMenu")
            isThisMainMenu = true;
        else
            isThisMainMenu = false;

        UpdateOnce();
    }

    private void Update()
    {
        if (!FPController)
            GetFPSControllerRef();
    }

    private void UpdateOnce()
    {
        if(isThisMainMenu)
        {
            mouseCursorOBJ.SetActive(false);
            gameManagerOBJ.SetActive(false);
        }
        else
        {
            mouseCursorOBJ.SetActive(true);
            saveManagerOBJ.SetActive(true);
            gameManagerOBJ.SetActive(true);
        }
    }

    private void GetFPSControllerRef()
    {
        FPController = GameObject.Find("FPSController");
    }
}
