using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dinosaur : MonoBehaviour
{
    public Vector3 EntryPoint { get; set; }
    public Vector3 WaitForFoodPoint { get; set; }
    public Vector3 ExitPoint { get; set; }
    public GameObject player { get; set; }

    #region AiData
    [HideInInspector] public NavMeshAgent agent;
    [Range(0, 100)] public float walkSpeed;
    [Range(1, 100)] public float angrySpeed;
    [Range(1, 00)] public float walkRadius;

    private float loopTime = 10.0f;
    private float hungryFactor = 1.0f;
    public bool isAngry { get; private set; } = false;
    public GameObject SmokeParticleEffect;

    private float rotationSpeed = 0.07f;
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
    }

    private void Start()
    {
        stateMachine.InitStateMachine();
    }

    private void Update()
    {
        stateMachine.UpdateStateMachine();
        timer.UpdateTimer();

        if(stateMachine.CurrentState != DinosaurStateMachine.Exit)
        {
            FaceTarget(player.transform.position);
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
        stateMachine.OnWaitForOrderEnter += () => { timer.OnTimeIsUpLogic += () => { isAngry = true; }; };
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
        stateMachine.CurrentState = DinosaurStateMachine.Walk;
        if (CheckIfDestinationReached())
        {
            if (timer.Iterations < 0)
            {
                agent.SetDestination(GenerateRandomNavMeshPos());
            }
            else
            {
                agent.SetDestination(WaitForFoodPoint);
            }
        }
    }

    private void OnWaitForOrderLogic()
    {
        stateMachine.CurrentState = DinosaurStateMachine.WaitForOrder;
    }

    private void OnAngryLogic()
    {
        stateMachine.CurrentState = DinosaurStateMachine.Angry;
        StartCoroutine(BeAnry());
        agent.speed = angrySpeed;
    }

    private void OnExitLogic()
    {
        stateMachine.CurrentState = DinosaurStateMachine.Exit;
        if (CheckIfDestinationReached())
        {
            GameObject.Destroy(gameObject);
        }
    }

    //Condition check for state machine
    public bool IsWalkFinished()
    {
        return timer.Iterations >= 2;
    }

    public bool IsFoodRecieved()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timer.EndTimer();
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator BeAnry()
    {
        SoundManager.DinosaurRoar.Invoke();

        animator.SetTrigger("IsAngry");
        GameObject effect = GameObject.Instantiate<GameObject>(SmokeParticleEffect);
        ParticleSystem smoke = effect.GetComponentInChildren<ParticleSystem>();
        Transform model = gameObject.transform.GetChild(0);

        while (smoke.isPlaying == true)
        {
            effect.transform.eulerAngles = transform.eulerAngles + new Vector3(model.eulerAngles.x, 0, model.eulerAngles.z);
            effect.transform.position = new Vector3(transform.position.x, transform.position.y + model.position.y + 1.15f, transform.position.z) + (transform.forward * 0.82f);
            yield return null;
        }

        GameObject.Destroy(effect.gameObject);
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
    }
}
