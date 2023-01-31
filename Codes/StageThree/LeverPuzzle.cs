using UnityEngine;

/*
 * LeverPuzzle: This script is to be attached on any lever that is part of a puzzle
 */
public class LeverPuzzle : MonoBehaviour, ICanBeInteracted<GameObject>
{
    [SerializeField] private CheckLeverPuzzle checkThisPuzzle;
    [SerializeField] private char thisNumber;

    private AnimationForThis animForThis;
    private bool isSelected;

    private void Start()
    {
        animForThis = GetComponent<AnimationForThis>();
        isSelected = false;
    }

    public void MouseClickInteraction()
    {
        if(!isSelected)
        {
            animForThis.PlayAnimation("Selected");
            checkThisPuzzle.AddCurrentChar(thisNumber);

            isSelected = true;
        }
        else
        {
            animForThis.PlayAnimation("Unselected");
            checkThisPuzzle.RemoveCurrentChar(thisNumber);

            isSelected = false;
        }
    }

    // This is not a pickable item
    public void PickableInteraction(GameObject _whichHand)
    {
    }
}
