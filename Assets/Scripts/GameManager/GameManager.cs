using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class GameManager : MonoBehaviour
{
    //To Delete later
    int health = 0;

    #region StateMachine
    public static string CurrentState;

    //States
    public const string Wander = "Wander";
    public const string Attack = "Attack";
    public const string Stupid = "Stupid";
    public const string Charge = "Charge";
    public const string GetWhiped = "Whip";
    public const string Ragdoll = "Ragdoll";

    //OnEnter
    public static Action OnWanderEnter = () => { };
    public static Action OnAttackEnter = () => { };
    public static Action OnStupidEnter = () => { };
    public static Action OnChargeEnter = () => { };
    public static Action OnGetWhipedEnter = () => { };
    public static Action OnRagdollEnter = () => { };

    //OnExit
    public static Action OnWanderExit = () => { };
    public static Action OnAttackExit = () => { };
    public static Action OnStupidExit = () => { };
    public static Action OnChargeExit = () => { };
    public static Action OnGetWhipedExit = () => { };
    public static Action OnRagdollExit = () => { };

    private StateMachine stateMachine;

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();

        stateMachine.AddState(Wander, new State(
            onEnter: _ => OnWanderEnter.Invoke(),
            onLogic: _ => OnWanderLogic(),
            onExit: _ => OnWanderExit.Invoke()));
        stateMachine.AddState(Attack, new State(
            onEnter: _ => OnAttackEnter.Invoke(),
            onLogic: _ => OnAttackLogic(),
            onExit: _ => OnAttackExit.Invoke()));
        stateMachine.AddState(Stupid, new State(
            onEnter: _ => OnStupidEnter.Invoke(),
            onLogic: _ => OnStupidLogic(),
            onExit: _ => OnStupidExit.Invoke()));
        stateMachine.AddState(Charge, new State(
            onEnter: _ => OnChargeEnter.Invoke(),
            onLogic: _ => OnChargeLogic(),
            onExit: _ => OnChargeExit.Invoke()));
        stateMachine.AddState(GetWhiped, new State(
            onEnter: _ => OnGetWhipedEnter.Invoke(),
            onLogic: _ => OnGetWhipedLogic(),
            onExit: _ => OnGetWhipedExit.Invoke()));
        stateMachine.AddState(Ragdoll, new State(
            onEnter: _ => OnRagdollEnter.Invoke(),
            onLogic: _ => OnRagdollLogic(),
            onExit: _ => OnRagdollExit.Invoke()));

        stateMachine.AddTransition(Wander, Stupid, _ => IsWanderFinished());
        stateMachine.AddTransition(Stupid, Charge, _ => IsStupidFinished());
        stateMachine.AddTransition(Charge, Attack, _ => IsChargeFinished());
        stateMachine.AddTransitionFromAny(new Transition("", GetWhiped, t => (health <= 0)));
        stateMachine.AddTransition(GetWhiped, Ragdoll, _ => IsRagdollFinished());

        stateMachine.SetStartState(Wander);
        stateMachine.Init();
    }

    private void UpdateStateMachine()
    {
        stateMachine.OnLogic();
    }

    //Condition check for state transitions
    private bool IsWanderFinished()
    {
        return true;
    }

    private bool IsStupidFinished()
    {
        return true;
    }

    private bool IsChargeFinished()
    {
        return true;
    }

    private bool IsRagdollFinished()
    {
        return true;
    }

    //Logic for each state
    private void OnWanderLogic()
    {
    }

    private void OnAttackLogic()
    {
    }

    private void OnStupidLogic()
    {
    }

    private void OnChargeLogic()
    {
    }

    private void OnGetWhipedLogic()
    {
    }

    private void OnRagdollLogic()
    {
    }
    #endregion

    Respawnable r;
    private CountDownTimer timer;
    int wantedNumberOfIterations = 5;

    // Start is called before the first frame update
    void Start()
    {
        timer = new CountDownTimer(0.5f, true);
        timer.OnTimeIsUpLogic += () => { OnTimeIsUpLogic(); };
        timer.StartTimer();
        r = gameObject.GetComponent<Respawnable>();
        r.OnRespawnLogic += () => { AdditionalFeaturesSpawnTest(); };
    }

    private void OnTimeIsUpLogic()
    {
        Debug.Log("time is up");
    }

    // Update is called once per frame
    void Update()
    {
        timer.UpdateTimer();
        if (timer.Iterations == wantedNumberOfIterations)
        {
            timer.SetIsContinuous(false);
            Transform t = transform;
            r.InvokeRespawn(t, 2.0f);
            timer.SetDuration(5.0f);
            timer.StartTimer();
            timer.OnTimeIsUpLogic = () => { r.InvokeRespawn(); };
            timer.ResetIterations();
        }
    }

    public static IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }

    private void AdditionalFeaturesSpawnTest()
    {
        Debug.Log("Do extra stuff");
    }
}
