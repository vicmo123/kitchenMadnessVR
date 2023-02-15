using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSM;  

public class DinosaurStateMachine
{
    public DinosaurStateMachine()
    {
    }

    #region StateMachine
    public static string CurrentState { get; set; }

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

        stateMachine.AddTransition(Walk, WaitForOrder, _ => true);
        stateMachine.AddTransition(WaitForOrder, Angry, _ => false);
        stateMachine.AddTransition(WaitForOrder, Exit, _ => true);
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
