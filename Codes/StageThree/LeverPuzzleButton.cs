using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzleButton : MonoBehaviour, ICanBeInteracted<GameObject>
{
    [SerializeField] private CheckLeverPuzzle thisLeverPuzzle;
    [SerializeField] private Material greenMat;

    [Header("If Code Is Correct Then...")]
    [SerializeField] private GameObject afterOBJ;

    private AnimationForThis thisAnim;
    private Renderer thisRend;
    private Material[] thisMats;

    private bool isPuzzleCorrect;
    private bool isWaitDone;
    private bool isDoorOpen;

    private void Start()
    {
        thisAnim = GetComponent<AnimationForThis>();
        thisRend = GetComponent<Renderer>();

        thisMats = thisRend.materials;

        isPuzzleCorrect = false;
        isWaitDone = false;
        isDoorOpen = false;
    }

    private void Update()
    {
        if (isWaitDone)
        {
            thisAnim.PlayAnimation("Off");
            isWaitDone = false;
        }

        if (isPuzzleCorrect && !isDoorOpen)
        {
            afterOBJ.GetComponent<AnimationForThis>().PlayAnimation("Open");
            isDoorOpen = true;
        }
    }

    public void MouseClickInteraction()
    {
        if(thisLeverPuzzle.IsCodeCorrect())
        {
            thisAnim.PlayAnimation("On");
            thisMats[1] = greenMat;

            thisRend.materials = thisMats;

            isPuzzleCorrect = true;
        }
        else
        {
            thisAnim.PlayAnimation("On");
            StartCoroutine(WaitForThis(1f));
        }
    }

    private IEnumerator WaitForThis(float _time)
    {
        yield return new WaitForSeconds(_time);

        isWaitDone = true;
        StopAllCoroutines();
    }

    // This is not a pickable item
    public void PickableInteraction(GameObject _whichHand)
    {
    }
}
