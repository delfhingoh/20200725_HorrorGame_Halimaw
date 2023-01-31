using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLeverPuzzle : MonoBehaviour
{
    [SerializeField] private List<char> numberActivated;

    [SerializeField] private string numbers;

    public bool IsCodeCorrect()
    {
        numbers = "";

        foreach(char thisChar in numberActivated)
        {
            numbers += thisChar.ToString();

            Debug.Log(thisChar.ToString());
        }

        if (numbers == "24" || numbers == "42")
            return true;

        return false;
    }

    public void AddCurrentChar(char _thisChar)
    {
        numberActivated.Add(_thisChar);
    }

    public void RemoveCurrentChar(char _thisChar)
    {
        numberActivated.Remove(_thisChar);
    }
}
