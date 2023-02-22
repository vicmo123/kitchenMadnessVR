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
    public string CurrentState;
    

    //States
    public const string Walk = "Walk";
    public const string TargetSpotted = "TargetSpotted";
    public const string Hit = "Hit";
    public const string Exit = "Exit";

    //OnEnter
    public Action OnWalkEnter = () => { };
    public Action OnTargetSpottedEnter = () => { };
    public Action OnHitEnter = () => { };
    public Action OnExitEnter = () => { };

    //OnExit
    public Action OnWalkExit = () => { };
    public Action OnTargetSpottedExit = () => { };
    public Action OnHitExit = () => { };
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
        stateMachine.AddState(Exit, new State(
            onEnter: _ => OnExitEnter.Invoke(),
            onLogic: _ => OnExitLogic(),
            onExit: _ => OnExitExit.Invoke()));

        stateMachine.AddTransition(Walk, TargetSpotted, _ => IsFoodItemFound());
        stateMachine.AddTransition(TargetSpotted, Exit, _ => IsObjectPickedUp());
        stateMachine.AddTransitionFromAny(new Transition("", Hit, t => rat.isHit));
        stateMachine.AddTransitionFromAny(new Transition("", Exit, t => (rat.isBored == true && rat.objectPickedUp == false)));
        stateMachine.AddTransition(Hit, Exit, _ => true);

        OnTargetSpottedEnter += () => { SoundManager.FoodSpottedSound?.Invoke(); };
        OnExitEnter += () => { rat.agent.SetDestination(rat.FindClosestExit()); };

        stateMachine.SetStartState(Walk);
        stateMachine.Init();

        timer = new CountDownTimer(timeTillExit, false);
        timer.OnTimeIsUpLogic = () => { TimeIsUp(); };
        timer.StartTimer();
    }

    public void UpdateStateMachine()
    {
        if (rat.agent.enabled == true)
        {
            timer.UpdateTimer();
            stateMachine.OnLogic();
        }
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

    private void OnHitLogic()
    {
        CurrentState = Hit;
        rat.agent.speed = rat.scaredSpeed;
        rat.animCtrl.SetBool("IsHit", true);
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

    private bool IsFoodItemFound()
    {
        if (CurrentState == Walk)
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(rat.transform.position, rat.sightRadius, rat.transform.forward, rat.sightLenght, rat.layerMask);

            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].collider.gameObject.tag == "Food")
                {
                    if(sphereCastHits[i].collider.GetComponent<Pickupable>().isGrabbedByRat == false)
                    {
                        rat.agent.SetDestination(sphereCastHits[i].transform.position);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void TimeIsUp()
    {
        rat.isBored = true;
    }

    #endregion
}
