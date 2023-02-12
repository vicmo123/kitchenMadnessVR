using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatStateMachine
{
    private Rat rat;
    private float timeTillExit;
    private CountDownTimer timer;

    public RatStateMachine(Rat ratAgent, float _timeTillExit)
    {
        rat = ratAgent;
        timeTillExit = _timeTillExit;
    }

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
        stateMachine.AddTransition(TargetSpotted, Exit, _ => IsObjectPickedUp());
        stateMachine.AddTransitionFromAny(new Transition("", Hit, t => IsHit()));
        stateMachine.AddTransitionFromAny(new Transition("", Exit, t => (rat.isBored == true && rat.objectPickedUp == false)));
        stateMachine.AddTransition(Hit, Scared, _ => IsHitFinished());
        stateMachine.AddTransition(Scared, Exit, _ => true);

        stateMachine.SetStartState(Walk);
        stateMachine.Init();

        timer = new CountDownTimer(timeTillExit, false);
        timer.OnTimeIsUpLogic = () => { TimeIsUp(); };
        timer.StartTimer();
    }

    public void UpdateStateMachine()
    {
        timer.UpdateTimer();
        stateMachine.OnLogic();
        Debug.Log(CurrentState);
    }

    //OnLogic
    private void OnWalkLogic()
    {
        CurrentState = Walk;
        if (rat.CheckIfDestinationReached())
        {
            rat.agent.SetDestination(rat.GenerateRandomNavMeshPos());
        }
    }

    private void OnExitLogic()
    {
        CurrentState = Exit;
        if (rat.CheckIfDestinationReached())
        {
            GameObject.Destroy(rat.gameObject);
        }
    }

    private void OnScaredLogic()
    {
        CurrentState = Scared;
        rat.agent.SetDestination(rat.FindClosestExit());
        rat.agent.speed = rat.scaredSpeed;
    }

    private void OnHitLogic()
    {
        CurrentState = Hit;
        rat.isScared = true;
    }

    private void OnTargetSpottedLogic()
    {
        CurrentState = TargetSpotted;
        rat.agent.speed = rat.chaseSpeed;
    }

    //Condition check for state transitions
    private bool IsObjectPickedUp()
    {
        return rat.objectPickedUp;
    }

    private bool IsHitFinished()
    {
        return rat.isScared;
    }

    private bool IsFoodItemFound()
    {
        if (CurrentState == Walk)
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(rat.transform.position, rat.sightRadius, rat.transform.forward, rat.sightLenght, rat.layerMask);

            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].collider.gameObject.tag == "Player")
                {
                    Debug.Log("Hit");
                    rat.agent.SetDestination(sphereCastHits[i].transform.position);
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

    private void TimeIsUp()
    {
        rat.isBored = true;
    }

    #endregion
}