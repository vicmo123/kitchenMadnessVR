using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class GameManager : MonoBehaviour
{
    //To Delete later
    int lives = 5;

    #region StateMachine
    public static string CurrentState;

    //States
    public const string StartGame = "StartGame";
    public const string SetupRound = "SetupRound";
    public const string StartRound = "StartRound";
    public const string UpdateRound = "UpdateRound";
    public const string EndRound = "EndRound";
    public const string RestartRound = "RestartRound";
    public const string EndGame = "EndGame";

    //OnEnter
    public static Action OnStartGameEnter = () => { };
    public static Action OnSetupRoundEnter = () => { };
    public static Action OnStartRoundEnter = () => { };
    public static Action OnUpdateRoundEnter = () => { };
    public static Action OnEndRoundEnter = () => { };
    public static Action OnRestartRoundEnter = () => { };
    public static Action OnEndGameEnter = () => { };

    //OnExit
    public static Action OnStartGameExit = () => { };
    public static Action OnSetupRoundExit = () => { };
    public static Action OnStartRoundExit = () => { };
    public static Action OnUpdateRoundExit = () => { };
    public static Action OnEndRoundExit = () => { };
    public static Action OnRestartRoundExit = () => { };
    public static Action OnEndGameExit = () => { };

    private StateMachine stateMachine;

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();

        stateMachine.AddState(StartGame, new State(
            onEnter: _ => OnStartGameEnter.Invoke(),
            onLogic: _ => OnStartGameLogic(),
            onExit: _ => OnStartGameExit.Invoke()));
        stateMachine.AddState(SetupRound, new State(
            onEnter: _ => OnStartRoundEnter.Invoke(),
            onLogic: _ => OnSetupRoundLogic(),
            onExit: _ => OnSetupRoundExit.Invoke()));
        stateMachine.AddState(StartRound, new State(
            onEnter: _ => OnStartRoundEnter.Invoke(),
            onLogic: _ => OnStartRoundLogic(),
            onExit: _ => OnStartRoundExit.Invoke()));
        stateMachine.AddState(UpdateRound, new State(
            onEnter: _ => OnUpdateRoundEnter.Invoke(),
            onLogic: _ => OnUpdateRoundLogic(),
            onExit: _ => OnUpdateRoundExit.Invoke()));
        stateMachine.AddState(EndRound, new State(
            onEnter: _ => OnEndRoundEnter.Invoke(),
            onLogic: _ => OnEndRoundLogic(),
            onExit: _ => OnEndRoundExit.Invoke()));
        stateMachine.AddState(RestartRound, new State(
            onEnter: _ => OnRestartRoundEnter.Invoke(),
            onLogic: _ => OnRestartRoundLogic(),
            onExit: _ => OnRestartRoundExit.Invoke()));
        stateMachine.AddState(EndGame, new State(
            onEnter: _ => OnEndGameEnter.Invoke(),
            onLogic: _ => OnEndGameLogic(),
            onExit: _ => OnEndGameExit.Invoke()));

        stateMachine.AddTransition(StartGame, SetupRound, _ => IsStartGameFinished());
        stateMachine.AddTransition(SetupRound, StartRound, _ => IsSetupRoundFinished());
        stateMachine.AddTransition(StartRound, UpdateRound, _ => IsStartRoundFinished());
        stateMachine.AddTransition(UpdateRound, EndRound, _ => IsUpdateRoundFinished());
        stateMachine.AddTransition(EndRound, RestartRound, _ => IsEndRoundFinished());
        stateMachine.AddTransition(RestartRound, SetupRound, _ => IsRestartRoundFinished());
        stateMachine.AddTransitionFromAny(new Transition("", EndGame, t => (IsEndGameRequested())));
        stateMachine.AddTransitionFromAny(new Transition("", EndRound, t => (lives <= 0)));

        stateMachine.SetStartState(StartGame);
        stateMachine.Init();
    }

    private void UpdateStateMachine()
    {
        stateMachine.OnLogic();
    }

    //Condition check for state transitions
    private bool IsStartGameFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else
            return false;
    }

    private bool IsSetupRoundFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else
            return false;
    }

    private bool IsStartRoundFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else
            return false;
    }

    private bool IsUpdateRoundFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else
            return false;
    }
    private bool IsEndRoundFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else
            return false;
    }
    private bool IsRestartRoundFinished()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            return true;
        else
            return false;
    }
    private bool IsEndGameRequested()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            return true;
        else
            return false;
    }

    //Logic for each state
    private void OnStartGameLogic()
    {
        Debug.Log("StartGame");
        //Initialise all the stuff for the game to work
        //Posibility of main menu
        //If UI when option is clicked, progress to next state
    }

    private void OnSetupRoundLogic()
    {
        Debug.Log("SetupRound");
        //Render the truck and kitchen environement with tools
        //Player can move around the kitchen and mess around
        //When the start button located on the window door is clicked, progress to next state
    }

    private void OnStartRoundLogic()
    {
        Debug.Log("StartRound");
        //Initialise every thing for the current round
        //When every thing is done, start three second timer with sound feedback.
        //when timer is invoked, proceed to next state
    }

    private void OnUpdateRoundLogic()
    {
        Debug.Log("UpdateRound");
        if (Input.GetKeyDown(KeyCode.A))
            lives--;
        //Main game loop
        //When the player is out of stars(lives), end the round
    }

    private void OnEndRoundLogic()
    {
        Debug.Log("EndRound");
        if (Input.GetKeyDown(KeyCode.P))
            lives = 5;
        //The player will have the option to start a new game with input
        //If option to continue is chosen, will proceed to restart round
    }

    private void OnRestartRoundLogic()
    {
        Debug.Log("RestartRound");
        //Delete every thing
        //Then proceed to startRound
        //Will cycle until go to end game
    }

    private void OnEndGameLogic()
    {
        Debug.Log("endGame");
        //Once you are here, you can go to another state
        //Do necessary actions to end the game
    }
    #endregion

    Respawnable r;
    private CountDownTimer timer;
    int wantedNumberOfIterations = 5;

    // Start is called before the first frame update
    void Start()
    {
        InitStateMachine();
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
        UpdateStateMachine();
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
