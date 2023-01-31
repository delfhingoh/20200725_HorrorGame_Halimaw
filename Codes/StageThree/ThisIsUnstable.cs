using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisIsUnstable : MonoBehaviour
{
    [SerializeField] private float beforeBreakingTimer;
    [SerializeField] private GameObject unstableObject;

    private Rigidbody thisRigidbody;
    private Renderer thisRenderer;

    private bool isUnstable;
    private bool isFalling;

    private void Start()
    {
        thisRigidbody = unstableObject.GetComponent<Rigidbody>();
        thisRenderer = unstableObject.GetComponent<Renderer>();

        if (beforeBreakingTimer == 0f)
            beforeBreakingTimer = 2f;

        isUnstable = false;
        isFalling = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!isUnstable)
                StartCoroutine(UnstableObject());

            if (isUnstable && !isFalling)
                UnstableEffect();
        }
    }

    private IEnumerator UnstableObject()
    {
        yield return new WaitForSeconds(beforeBreakingTimer);

        isUnstable = true;
        StopCoroutine(UnstableObject());
    }

    private void UnstableEffect()
    {
        thisRenderer.material.color = Color.red;

        thisRigidbody.useGravity = true;
        thisRigidbody.constraints = RigidbodyConstraints.None;

        isFalling = true;
    }
}
