using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseThisOncePlayerIsThrough : MonoBehaviour
{
    [SerializeField] private GameObject closeThisOBJ;
    [SerializeField] private GameObject blockOBJ;

    private bool isClose;

    private void Start()
    {
        isClose = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == DoNotUnload.doNotUnload.FPController)
        {
            isClose = true;
            blockOBJ.SetActive(true);
        }
    }

    public bool GetIsClose()
    {
        return isClose;
    }

    public void SetIsClose(bool _close)
    {
        isClose = _close;
    }
}
