using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * EnemyManager: This script will handles the behaviour of Enemy.
 */
public class EnemyManager : MonoBehaviour
{  
    private enum EnemyState
    {
        IDLE,
        PATROL,
        CHASE
    }

    [SerializeField] private NavMeshAgent thisEnemyAgent;
    [SerializeField] private EnemyState thisState;
    [SerializeField] private GameObject thisTarget;
    [SerializeField] private float changeDirectionProability;
    [SerializeField] private GameObject[] patrolPoints;

    private FirstPersonManager FPManager;
    private AudioForThis thisAudio;
    private float distFromTarget;
    private float damageRange;

    private float playerStressLevel;
    private float waitTime;
    private int currentPatrolIndex;
    private bool isWaiting;
    private bool isForward;
    private bool isEnemyVeryNear;
    private bool isCausingStress;

    private bool isThisLoaded;

    private void Start()
    {
        thisTarget = DoNotUnload.doNotUnload.FPController;
        FPManager = thisTarget.GetComponent<FirstPersonManager>();
        thisState = EnemyState.PATROL;
        thisAudio = GetComponent<AudioForThis>();
        isWaiting = false;
        isForward = true;
        isEnemyVeryNear = false;
        isCausingStress = false;
        isThisLoaded = false;

        damageRange = 10f;
        waitTime = 3f;
        currentPatrolIndex = 0;

        SetPatrolDestination();
    }

    private void Update()
    {
        if (!thisTarget)
        {
            thisTarget = DoNotUnload.doNotUnload.FPController;
            FPManager = thisTarget.GetComponent<FirstPersonManager>();
            thisAudio = GetComponent<AudioForThis>();

            damageRange = 10f;
        }
        else
        {
            if(SaveManager.saveManager.isSaveFileLoaded && !isThisLoaded)
            {
                thisTarget = DoNotUnload.doNotUnload.FPController;
                FPManager = thisTarget.GetComponent<FirstPersonManager>();
                thisAudio = GetComponent<AudioForThis>();

                damageRange = 10f;

                isThisLoaded = true;
            }

            if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage2" ||
                ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage3")
            {
                if (!thisAudio.GetThisAudioSource().isPlaying)
                    thisAudio.PlayThisSoundOnce("clapping");
            }
            else
            {
                thisAudio.GetThisAudioSource().Stop();
            }
            

            distFromTarget = DistanceBetweenThis(thisTarget, this.gameObject);

            if (isCausingStress)
                IncreasePlayerStress();
            else
                DecreasePlayerStress();
        }

        EnemyBehaviour();
    }

    private void FixedUpdate()
    {
        if(!FPManager)
        {
            thisTarget = DoNotUnload.doNotUnload.FPController;
            FPManager = thisTarget.GetComponent<FirstPersonManager>();
        }

        if (FPManager.GetIsVignetteShown())
        {
            FPManager.GetVignetteOverlay().intensity.value = (damageRange - distFromTarget) * 0.2f;
        }
        else
        {
            FPManager.GetVignetteOverlay().intensity.value = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == thisTarget && !FPManager.GetIsPlayerHiding())
        {
            thisState = EnemyState.CHASE;
            distFromTarget = DistanceBetweenThis(other.gameObject, this.gameObject);

            if (distFromTarget < damageRange)
                FPManager.SetIsVignetteShown(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == thisTarget && !FPManager.GetIsPlayerHiding())
        {
            thisState = EnemyState.CHASE;
            distFromTarget = DistanceBetweenThis(other.gameObject, this.gameObject);

            if (distFromTarget < damageRange)
                FPManager.SetIsVignetteShown(true);

            if (thisEnemyAgent.remainingDistance < (damageRange * 0.2f))
                isEnemyVeryNear = true;
            else
                isEnemyVeryNear = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FPManager.SetIsVignetteShown(false);

        thisState = EnemyState.IDLE;
        isEnemyVeryNear = false;
    }

    private void IncreasePlayerStress()
    {
        playerStressLevel = FPManager.GetStressLevel();

        if ((int)playerStressLevel < FPManager.GetMaxStressLevel())
        {
            if(!isEnemyVeryNear)
                playerStressLevel += Time.deltaTime * 2f;
            else
                playerStressLevel += Time.deltaTime * 4f;

            FPManager.SetStressLevel(playerStressLevel);
            FPManager.SetStressSlider(playerStressLevel);
        }
    }

    private void DecreasePlayerStress()
    {
        playerStressLevel = FPManager.GetStressLevel();

        if ((int)playerStressLevel > 0)
        {
            playerStressLevel -= Time.deltaTime * 2f;

            FPManager.SetStressLevel(playerStressLevel);
            FPManager.SetStressSlider(playerStressLevel);
        }
    }

    private void EnemyBehaviour()
    {
        if(thisState == EnemyState.IDLE)
        {
            isWaiting = true;
            isCausingStress = false;
            thisEnemyAgent.SetDestination(this.gameObject.transform.position);

            if (isWaiting)
            {
                if (waitTime > 0f)
                    waitTime -= Time.deltaTime * 1f;
                else
                {
                    waitTime = 3f;
                    isWaiting = false;

                    thisState = EnemyState.PATROL;
                }
            }
        }
        else if (thisState == EnemyState.PATROL)
        {
            isCausingStress = false;

            if (thisEnemyAgent.remainingDistance < 1f)
            {
                ChangePatrolPoint();
                SetPatrolDestination();
            }
        }
        else if(thisState == EnemyState.CHASE)
        {
            if (!FPManager.GetIsPlayerHiding())
            {
                thisEnemyAgent.SetDestination(thisTarget.transform.position);
                isCausingStress = true;
            }
            else
            {
                thisState = EnemyState.PATROL;
                isCausingStress = false;
            }
        }
    }

    private void ChangePatrolPoint()
    {
        if(Random.Range(0f, 1f) <= changeDirectionProability)
        {
            isForward = !isForward;
        }

        if(isForward)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
        else
        {
            currentPatrolIndex--;

            if(currentPatrolIndex < 0)
            {
                currentPatrolIndex = 0;
            }
        }
    }

    private void SetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;
        thisEnemyAgent.SetDestination(destination);
    }

    private float DistanceBetweenThis(GameObject thisOBJ, GameObject thatOBJ)
    {
        return Vector3.Distance(thisOBJ.transform.position, thatOBJ.transform.position);
    }

    public int GetThisState()
    {
        return (int)thisState;
    }

    public void SetThisState(int _state)
    {
        thisState = (EnemyState)_state;
    }

    public int GetCurrentPatrolIndex()
    {
        return currentPatrolIndex;
    }

    public void SetCurrentPatrolIndex(int _index)
    {
        currentPatrolIndex = _index;
    }

    public NavMeshAgent GetThisAgent()
    {
        return thisEnemyAgent;
    }
}
