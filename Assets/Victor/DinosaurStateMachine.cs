using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;  

public class DinosaurStateMachine
{
    Dinosaur dino;

    public DinosaurStateMachine(Dinosaur _dino)
    {
        dino = _dino;
    }

    #region StateMachine
    public string CurrentState { get; set; }

    //States
    public const string Walk = "Walk";
    public const string WaitForOrder = "WaitForOrder";
    public const string Angry = "Angry";
    public const string Exit = "Exit";

    //OnEnter
    public Action OnWalkEnter = () => { };
    public Action OnWaitForOrderEnter = () => { };
    public Action OnAngryEnter = () => { };
    public Action OnExitEnter = () => { };

    //OnLogic
    public Action OnWalkLogic = () => { };
    public Action OnWaitForOrderLogic = () => { };
    public Action OnAngryLogic = () => { };
    public Action OnExitLogic = () => { };

    //OnExit
    public Action OnWalkExit = () => { };
    public Action OnWaitForOrderExit = () => { };
    public Action OnAngryExit = () => { };
    public Action OnExitExit = () => { };

    public StateMachine stateMachine;

    public void InitStateMachine()
    {
        stateMachine = new StateMachine();

        stateMachine.AddState(Walk, new State(
            onEnter: _ => OnWalkEnter.Invoke(),
            onLogic: _ => OnWalkLogic.Invoke(),
            onExit: _ => OnWalkExit.Invoke()));
        stateMachine.AddState(WaitForOrder, new State(
            onEnter: _ => OnWaitForOrderEnter.Invoke(),
            onLogic: _ => OnWaitForOrderLogic.Invoke(),
            onExit: _ => OnWaitForOrderExit.Invoke()));
        stateMachine.AddState(Angry, new State(
            onEnter: _ => OnAngryEnter.Invoke(),
            onLogic: _ => OnAngryLogic.Invoke(),
            onExit: _ => OnAngryExit.Invoke()));
        stateMachine.AddState(Exit, new State(
            onEnter: _ => OnExitEnter.Invoke(),
            onLogic: _ => OnExitLogic.Invoke(),
            onExit: _ => OnExitExit.Invoke()));

        stateMachine.AddTransition(Walk, WaitForOrder, _ => dino.IsWalkFinished());
        stateMachine.AddTransition(WaitForOrder, Exit, _ => dino.IsFoodRecieved());
        stateMachine.AddTransition(WaitForOrder, Angry, _ => dino.isAngry);
        stateMachine.AddTransition(Angry, Exit, _ => true);

        stateMachine.SetStartState(Walk);
        stateMachine.Init();
    }

    public void UpdateStateMachine()
    {
        stateMachine.OnLogic();
    }
    #endregion
}
