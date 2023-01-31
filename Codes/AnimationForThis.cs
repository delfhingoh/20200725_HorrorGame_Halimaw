using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * AnimationForThis: This is a script to be attached on any object that
 * has animation which requires a certain trigger.
 */
 [RequireComponent(typeof(Animator))]
public class AnimationForThis : MonoBehaviour
{
    [SerializeField] private Animator thisAnimator;
    [SerializeField] private bool isThisAChild;

    private void Start()
    {
        if (!isThisAChild && !thisAnimator)
            thisAnimator = GetComponent<Animator>();
        else if (isThisAChild && !thisAnimator)
            thisAnimator = this.transform.parent.GetComponent<Animator>();
    }

    public void PlayAnimation(string _triggerName)
    {
        thisAnimator.SetTrigger(_triggerName);
    }

    public void SetThisAnimationSpeed(float _speed)
    {
        thisAnimator.speed = _speed;
    }
}
