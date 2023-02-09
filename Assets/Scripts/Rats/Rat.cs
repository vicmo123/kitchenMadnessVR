using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;


public class Rat : MonoBehaviour
{
    #region StateMachine
    public static string CurrentState;

    //States
    public const string Walk = "Walk";
    public const string TargetSpotted = "TargetSpotted";
    public const string Hit = "Hit";
    public const string Scared = "Scared";
    public const string Exit = "Exit";

    //OnEnter
    public Action OnWalkEnter = () => { };
    public Action OnTargetSpottedEnter = () => { };
    public Action OnHitEnter = () => { };
    public Action OnScaredEnter = () => { };
    public Action OnExitEnter = () => { };

    //OnExit
    public Action OnWalkExit = () => { };
    public Action OnTargetSpottedExit = () => { };
    public Action OnHitExit = () => { };
    public Action OnScaredExit = () => { };
    public Action OnExitExit = () => { };

    public StateMachine stateMachine;

    public void InitStateMachine()
    {
        stateMachine = new StateMachine();

        stateMachine.AddState(Walk, new State(
            onEnter: _ => OnWalkEnter.Invoke(),
            onLogic: _ => OnWalkLogic(),
            onExit: _ => OnWalkExit.Invoke()));
        stateMachine.AddState(TargetSpotted, new State(
            onEnter: _ => OnTargetSpottedEnter.Invoke(),
            onLogic: _ => OnTargetSpottedLogic(),
            onExit: _ => OnTargetSpottedExit.Invoke()));
        stateMachine.AddState(Hit, new State(
            onEnter: _ => OnHitEnter.Invoke(),
            onLogic: _ => OnHitLogic(),
            onExit: _ => OnHitExit.Invoke()));
        stateMachine.AddState(Scared, new State(
            onEnter: _ => OnScaredEnter.Invoke(),
            onLogic: _ => OnScaredLogic(),
            onExit: _ => OnScaredExit.Invoke()));
        stateMachine.AddState(Exit, new State(
            onEnter: _ => OnExitEnter.Invoke(),
            onLogic: _ => OnExitLogic(),
            onExit: _ => OnExitExit.Invoke()));

        stateMachine.AddTransition(Walk, TargetSpotted, _ => IsFoodItemFound());
        stateMachine.AddTransition(TargetSpotted, Exit, _ => IsTargetReached());
        stateMachine.AddTransitionFromAny(new Transition("", Hit, t => IsHit()));
        stateMachine.AddTransition(Hit, Scared, _ => IsHitFinished());
        stateMachine.AddTransition(Scared, Exit, _ => IsScaredFinished());

        stateMachine.SetStartState(Walk);
        stateMachine.Init();
    }


    private void OnWalkLogic()
    {
        CurrentState = Walk;
        if (CheckIfDestinationReached())
        {
            agent.SetDestination(GenerateRandomNavMeshPos());
        }
    }

    private void OnExitLogic()
    {
        CurrentState = Exit;
        if (CheckIfDestinationReached())
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnScaredLogic()
    {
        CurrentState = Scared;
        agent.SetDestination(new Vector3(0, 0.0f, 0));
        agent.speed = 100.0f;
    }

    private void OnHitLogic()
    {
        CurrentState = Hit;
        isScared = true;
    }

    private void OnTargetSpottedLogic()
    {
        CurrentState = TargetSpotted;
        agent.speed = chaseSpeed;
    }

    public void UpdateStateMachine()
    {
        stateMachine.OnLogic();
        Debug.Log(CurrentState);
    }

    //Condition check for state transitions
    private bool IsTargetReached()
    {
        if(CurrentState == TargetSpotted && CheckIfDestinationReached())
        {
            isScared = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsHitFinished()
    {
        return isScared;
    }

    private bool IsScaredFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else
            return false;
    }

    private bool IsFoodItemFound()
    {
        if(CurrentState == Walk)
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(transform.position, 3.0f, transform.forward, sightLenght, layerMask);

            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].collider.gameObject.tag == "Player")
                {
                    Debug.Log("Hit");
                    agent.SetDestination(sphereCastHits[i].transform.position);
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsHit()
    {
        if (Input.GetKeyDown(KeyCode.A))
            return true;
        else
            return false;
    }
    #endregion

    #region AiData
    [HideInInspector] public NavMeshAgent agent;
    [Range(0, 100)] public float walkSpeed;
    [Range(0, 100)] public float chaseSpeed;
    [Range(1, 500)] public float walkRadius;
    [Range(0, 100)] public float ChaseDistance;
    [Range(1, 20)] public float sightLenght;
    [Range(1, 20)] public float sightRadius;
    [HideInInspector] public bool isScared;
    int layerMask;
    #endregion

    private void Awake()
    {
        layerMask = LayerMask.GetMask("UI");
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = walkSpeed;
            agent.SetDestination(GenerateRandomNavMeshPos());
        }
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        UpdateStateMachine();
    }

    private Vector3 GenerateRandomNavMeshPos()
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

    private bool CheckIfDestinationReached()
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
}
