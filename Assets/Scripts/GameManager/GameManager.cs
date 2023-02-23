using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class GameManager : MonoBehaviour
{
    #region StateMachine
    public static string CurrentState;

    //States
    public const string StartGame = "StartGame";
    public const string SetupRound = "SetupRound";
    public const string StartRound = "StartRound";
    public const string UpdateRound = "UpdateRound";
    public const string RestartRound = "RestartRound";
    public const string EndGame = "EndGame";

    //OnEnter
    public static Action OnStartGameEnter = () => { };
    public static Action OnSetupRoundEnter = () => { };
    public static Action OnStartRoundEnter = () => { };
    public static Action OnUpdateRoundEnter = () => { };
    public static Action OnRestartRoundEnter = () => { };
    public static Action OnEndGameEnter = () => { };

    //OnExit
    public static Action OnStartGameExit = () => { };
    public static Action OnSetupRoundExit = () => { };
    public static Action OnStartRoundExit = () => { };
    public static Action OnUpdateRoundExit = () => { };
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
        stateMachine.AddState(RestartRound, new State(
            onEnter: _ => OnRestartRoundEnter.Invoke(),
            onLogic: _ => OnRestartRoundLogic(),
            onExit: _ => OnRestartRoundExit.Invoke()));
        stateMachine.AddState(EndGame, new State(
            onEnter: _ => OnEndGameEnter.Invoke(),
            onLogic: _ => OnEndGameLogic(),
            onExit: _ => OnEndGameExit.Invoke()));

        stateMachine.AddTransition(StartGame, SetupRound, _ => true);
        stateMachine.AddTransition(SetupRound, StartRound, _ => isRoundActive);
        stateMachine.AddTransition(StartRound, UpdateRound, _ => true);
        stateMachine.AddTransition(RestartRound, SetupRound, _ => true);
        stateMachine.AddTransitionFromAny(new Transition("", EndGame, t => (IsEndGameRequested())));
        stateMachine.AddTransitionFromAny(new Transition("", RestartRound, t => (boardManager.GetCurrentNbStars() <= 0)));

        OnUpdateRoundEnter += () => { timer.Reset(); };
        OnUpdateRoundEnter += () => { ingredientSpawner.RoundStarting(); };
        OnUpdateRoundEnter += () => { rats.StartRound(); };
        OnUpdateRoundExit += () => { rats.EndRound(); };
        OnUpdateRoundEnter += () =>
        {
            dinoManager.roundActive = true;
            boardManager.roundActive = true;
            boardManager.GenerateOrder();
        };
        OnUpdateRoundExit += () =>
        {
            SoundManager.GameOver.Invoke();
            dinoManager.roundActive = false;
            boardManager.roundActive = false;
        };

        stateMachine.SetStartState(StartGame);
        stateMachine.Init();
    }

    private void UpdateStateMachine()
    {
        stateMachine.OnLogic();
    }

    //Condition check for state transitions
    private bool IsEndGameRequested()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            return true;
        else
            return false;
    }
    #endregion

    //Game object links
    public RatsManager rats;
    public BoardManager boardManager;
    public StartGameUi startGameUi;
    public IngredientSpawner ingredientSpawner;
    public DinosaurManager dinoManager;

    //GameFlow variables
    [SerializeField, Range(1.0f, 10.0f)] float timeBeforeRoundStarts = 3.0f;
    private CountDownTimer countDownTimer;
    private Timer timer;
    [SerializeField, Range(1, 10)] private int maxNumberOfStars = 5;
    public int currentNumberOfStars { get; private set; }
    public bool isRoundActive { get; private set; }

    private void Awake()
    {
        currentNumberOfStars = maxNumberOfStars;
        InitStateMachine();
    }
    
    private void Start()
    {
        startGameUi.StartButtonClickedEvent.AddListener(ActivateRound);
        countDownTimer = new CountDownTimer(3.0f, false);
        timer = new Timer();

        

        SoundManager.MainTheme?.Invoke();
        isRoundActive = true;
    }
   
    private void Update()
    {
        UpdateStateMachine();
        //Calculate the time elapsed since beginning of round will reset automaticlly when the round restarts
        timer.UpdateTimer();
    }

    private void ActivateRound()
    {
        isRoundActive = true;
    }

    //Logic for each state
    private void OnStartGameLogic()
    {
        CurrentState = StartGame;
        //Initialise all the stuff for the game to work
        //Posibility of main menu
        //If UI when option is clicked, progress to next state
    }

    private void OnSetupRoundLogic()
    {
        CurrentState = SetupRound;
        //Render the truck and kitchen environement with tools
        //Player can move around the kitchen and mess around
        //When the start button located on the window door is clicked, progress to next state
    }

    private void OnStartRoundLogic()
    {
        CurrentState = StartRound;
        //Initialise every thing for the current round
        //When every thing is done, start three second timer with sound feedback.
        //when timer is invoked, proceed to next state
    }

    private void OnUpdateRoundLogic()
    {
        CurrentState = UpdateRound;

        //Main game loop
        if (Input.GetKeyDown(KeyCode.M))
        {
            currentNumberOfStars--;
        }

        boardManager.ElapsedTime = timer.Elapsed;
        rats.timeElapsedRound = timer.Elapsed;
       
    }

    private void OnRestartRoundLogic()
    {
        CurrentState = RestartRound;
        currentNumberOfStars = maxNumberOfStars;
        isRoundActive = false;
        startGameUi.gameObject.SetActive(true);
        startGameUi.ResetUi();

        //The player will have the option to start a new game with input
        //If option to continue is chosen, will proceed to restart round
        //Delete every thing
        //Then proceed to startRound
        //Will cycle until go to end game
    }

    private void OnEndGameLogic()
    {
        CurrentState = EndGame;

        //Once you are here, you cannot go to another state
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
