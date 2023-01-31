using UnityEngine;
using UnityEngine.UI;

public class InputTipScript : MonoBehaviour
{
    [SerializeField] private Text tipText;

    public Text GetTipText()
    {
        return tipText;
    }

    public void SetTipText(string _tipText)
    {
        if (tipText)
            tipText.text = _tipText;
    }

    public void ClearTipText()
    {
        if (tipText)
            tipText.text = "";
    }
}
