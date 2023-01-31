using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * DigitalSafeScript: This script is to be on the interactable digital safe.
 * This script handles the behaviours and interactions of the safe.
 */
public class DigitalSafeScript : MonoBehaviour, ICanBeInteracted<GameObject>
{
    [Header("")]
    [Header("DIGITAL SAFE")]
    [SerializeField] private AnimationForThis animationForThis;
    [SerializeField] private AudioForThis audioForThis;

    [SerializeField] private GameObject thisUIScreen;
    [SerializeField] private Image codeTextAreaImage;
    [SerializeField] private Text codeTextArea;
    [SerializeField] private string codeNumbers;

    private GameObject selectedOBJ;
    private AudioClip thisClip;
    private string codeText;

    private void Start()
    {
        codeTextArea.text = "";
        codeText = "";

        if (!audioForThis)
            audioForThis = GetComponent<AudioForThis>();
        if (!animationForThis)
            animationForThis = GetComponent<AnimationForThis>();
    }

    // User can interact with Digital Safe by clicking on object
    public void MouseClickInteraction()
    {
        ShowUIScreen();
    }

    // Digital Safe is not a pickable object.
    public void PickableInteraction(GameObject whichHand)
    {
    }

    private void ShowUIScreen()
    {
        if(codeText.Length > 0)
        {
            codeTextArea.text = "";
            codeText = "";
        }

        DoNotUnload.doNotUnload.LockPlayerMovement();
        DoNotUnload.doNotUnload.UnlockMouseCursor();

        thisUIScreen.SetActive(true);
    }

    public void CloseUIScreen()
    {
        DoNotUnload.doNotUnload.LockMouseCursor();
        DoNotUnload.doNotUnload.UnlockPlayerMovement();

        thisUIScreen.SetActive(false);
    }

    public void PressNumber()
    {
        audioForThis.PlayThisSoundOnce("DigitalBeepSound");

        selectedOBJ = EventSystem.current.currentSelectedGameObject;
        codeText = codeText + selectedOBJ.name;

        codeTextArea.text = codeText;
    }

    public void Enter()
    {
        // Check Code
        if (codeTextArea.text == codeNumbers)
        {
            CloseUIScreen();
            animationForThis.PlayAnimation("DigitalSafeOpen");

            audioForThis.PlayThisSoundOnce("SafeOpenSound");
        }
        else
        {
            StartCoroutine(WaitForThis());
        }
    }

    public void Delete()
    {
        audioForThis.PlayThisSoundOnce("DigitalBeepSound");

        if (codeText.Length > 0)
        {
            codeText = codeText.Substring(0, codeText.Length - 1);
            codeTextArea.text = codeText;
        }
    }

    public IEnumerator WaitForThis()
    {
        audioForThis.PlayThisSoundOnce("DigitalBeepSound");
        thisClip = audioForThis.GiveMeThisAudioClip("DigitalBeepSound");
        yield return new WaitForSeconds(thisClip.length);

        audioForThis.PlayThisSoundOnce("ErrorSignalSound");
        thisClip = audioForThis.GiveMeThisAudioClip("ErrorSignalSound");
        codeTextAreaImage.color = Color.red;
        yield return new WaitForSeconds(thisClip.length);

        codeTextAreaImage.color = Color.black;
        StopCoroutine(WaitForThis());
    }
}
