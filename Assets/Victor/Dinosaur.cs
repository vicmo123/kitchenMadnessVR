using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dinosaur : MonoBehaviour
{
    public Vector3 EntryPoint { get; set; }
    public Vector3 WaitForFoodPoint { get; set; }
    public Vector3 ExitPoint { get; set; }

    #region AiData
    [HideInInspector] public NavMeshAgent agent;
    [Range(0, 100)] public float walkSpeed;
    [Range(1, 100)] public float angrySpeed;
    [Range(1, 00)] public float walkRadius;

    private float loopTime = 10.0f;
    private float hungryFactor = 1.0f;
    [HideInInspector] public bool isAngry = false;
    public GameObject SmokeParticleEffect;
    #endregion

    private DinosaurStateMachine stateMachine;
    private CountDownTimer timer;
    public Animator animator;

    private void Awake()
    {
        timer = new CountDownTimer(loopTime * hungryFactor, true);

        stateMachine = new DinosaurStateMachine(this);
        agent = GetComponent<NavMeshAgent>();
        SetStateMachineActions();

        timer.OnTimeIsUpLogic = () =>
        {
            hungryFactor -= 0.1f;
            timer.SetDuration(loopTime * hungryFactor);
            DinosaurMakeSound();
            Debug.Log("Ding");
            Debug.Log(loopTime * hungryFactor);
        };
    }
    private void Start()
    {
        stateMachine.InitStateMachine();
    }

    private void Update()
    {
        stateMachine.UpdateStateMachine();
        timer.UpdateTimer();
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(BeAnry());
        }
    }

    private void SetStateMachineActions()
    {
        stateMachine.OnWalkEnter += () =>
        {
            timer.StartTimer();

            if (agent != null)
            {
                agent.Warp(EntryPoint);
                agent.speed = walkSpeed;
                agent.SetDestination(GenerateRandomNavMeshPos());
            }
        };
        stateMachine.OnWaitForOrderEnter += () => { agent.SetDestination(WaitForFoodPoint); };
        stateMachine.OnAngryEnter += () => { agent.speed = angrySpeed; };
        stateMachine.OnExitEnter += () => { agent.SetDestination(ExitPoint); };

        stateMachine.OnWalkLogic += () => { OnWalkLogic(); };
        stateMachine.OnWaitForOrderLogic += () => { OnWaitForOrderLogic(); };
        stateMachine.OnAngryLogic += () => { OnAngryLogic(); };
        stateMachine.OnExitLogic += () => { OnExitLogic(); };
    }

    public bool CheckIfDestinationReached()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Vector3 GenerateRandomNavMeshPos()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    //OnLogic for stateMachine
    private void OnWalkLogic()
    {
        if (CheckIfDestinationReached())
        {
            agent.SetDestination(GenerateRandomNavMeshPos());
        }
    }

    private void OnWaitForOrderLogic()
    {

    }

    private void OnAngryLogic()
    {

    }

    private void OnExitLogic()
    {
        if (CheckIfDestinationReached())
        {
            GameObject.Destroy(gameObject);
        }
    }

    //Condition check for state machine
    public bool IsWalkFinished()
    {
        return timer.Iterations >= 1;
    }

    public bool IsWaitForFoodFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        else {
            return false;
        }
    }

    public bool IsAngryFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DinosaurMakeSound()
    {
        SoundManager.DinosaurRoar.Invoke();
    }

    private IEnumerator BeAnry()
    {
        animator.SetTrigger("IsAngry");
        GameObject effect = GameObject.Instantiate<GameObject>(SmokeParticleEffect);
        
        while (effect.activeInHierarchy == true)
        {
            effect.transform.eulerAngles = transform.eulerAngles;
            effect.transform.position = new Vector3(transform.position.x, 1.45f, transform.position.z) + (transform.forward * 0.82f);
            yield return null;
        }

        isAngry = true;
    }
}
