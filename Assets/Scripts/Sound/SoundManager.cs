using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    //Main
    public static Action GameOver;
    public static Action MainTheme;
    public static Action LooseStar;
    public static Action ButtonClick;
    public static Action MoveOrder;
    public static Action GoodJob;

    //Cooking
    public static Action BellDing;
    public static Action Chopping;
    public static Action Grilling;
    public static Action Sclicing;
    public static Action FoodDroped;
    public static Action ToolDropped;
    public static Action TreadmillSound;
    public static Action CatchFire;
    public static Action SauceSquirt;
    public static Action FoodFallsInTrash;

    //Rats
    public static Action SpawnSqueek;
    public static Action FoodSpottedSound;
    public static Action RatLaugh;

    //Dinosaurs
    public static Action DinosaurRoar;

    //Audio Source
    [HideInInspector] public AudioSource GameOverSource;
    [HideInInspector] public AudioSource MainThemeSource;
    [HideInInspector] public AudioSource LooseStarSource;
    [HideInInspector] public AudioSource ButtonClickSource;
    [HideInInspector] public AudioSource MoveOrderSource;
    [HideInInspector] public AudioSource GoodJobSource;
    [HideInInspector] public AudioSource BellDingSource;
    [HideInInspector] public AudioSource ChoppingSource;
    [HideInInspector] public AudioSource GrillingSource;
    [HideInInspector] public AudioSource SclicingSource;
    [HideInInspector] public AudioSource FoodDropedSource;
    [HideInInspector] public AudioSource ToolDroppedSource;
    [HideInInspector] public AudioSource TreadmillSoundSource;
    [HideInInspector] public AudioSource SpawnSqueekSource;
    [HideInInspector] public AudioSource FoodSpottedSoundSource;
    [HideInInspector] public AudioSource RatLaughSource;
    [HideInInspector] public AudioSource DinosaurRoarSource;
    [HideInInspector] public AudioSource CatchFireSource;
    [HideInInspector] public AudioSource SauceSquirtSource;
    [HideInInspector] public AudioSource FoodFallsInTrashSource;

    [Tooltip("Link to AudioSource prefabs in the assets, the script will load them in the scene when the game starts")] public SoundData Data;

    private void Awake()
    {
        GameOverSource = Instantiate(Data.GameOver, transform);
        MainThemeSource = Instantiate(Data.MainTheme, transform);
        LooseStarSource = Instantiate(Data.LooseStar, transform);
        ButtonClickSource = Instantiate(Data.ButtonClick, transform);
        MoveOrderSource = Instantiate(Data.MoveOrder, transform);
        GoodJobSource = Instantiate(Data.GoodJob, transform);
        BellDingSource = Instantiate(Data.BellDing, transform);
        ChoppingSource = Instantiate(Data.Chopping, transform);
        GrillingSource = Instantiate(Data.Grilling, transform);
        SclicingSource = Instantiate(Data.Sclicing, transform);
        FoodDropedSource = Instantiate(Data.FoodDroped, transform);
        ToolDroppedSource = Instantiate(Data.ToolDropped, transform);
        TreadmillSoundSource = Instantiate(Data.TreadmillSound, transform);
        SpawnSqueekSource = Instantiate(Data.SpawnSqueek, transform);
        FoodSpottedSoundSource = Instantiate(Data.FoodSpottedSound, transform);
        RatLaughSource = Instantiate(Data.RatLaugh, transform);
        DinosaurRoarSource = Instantiate(Data.DinosaurRoar, transform);
        CatchFireSource = Instantiate(Data.CatchFire, transform);
        SauceSquirtSource = Instantiate(Data.SauceSquirt, transform);
        FoodFallsInTrashSource = Instantiate(Data.FoodFallsInTrash, transform);

        GameOver += () => { PlaySound(GameOverSource); };
        MainTheme += () => { PlaySound(MainThemeSource); };
        LooseStar += () => { PlaySound(LooseStarSource); };
        ButtonClick += () => { PlaySound(ButtonClickSource); };
        MoveOrder += () => { PlaySound(MoveOrderSource); };
        GoodJob += () => { PlaySound(GoodJobSource); };
        BellDing += () => { PlaySound(BellDingSource); };
        Chopping += () => { PlaySound(ChoppingSource); };
        Grilling += () => { PlaySound(GrillingSource); };
        Sclicing += () => { PlaySound(SclicingSource); };
        FoodDroped += () => { PlaySound(FoodDropedSource); };
        ToolDropped += () => { PlaySound(ToolDroppedSource); };
        TreadmillSound += () => { PlaySound(TreadmillSoundSource); };
        SpawnSqueek += () => { PlaySound(SpawnSqueekSource); };
        FoodSpottedSound += () => { PlaySound(FoodSpottedSoundSource); };
        RatLaugh += () => { PlaySound(RatLaughSource); };
        DinosaurRoar += () => { PlaySound(DinosaurRoarSource); };
        CatchFire += () => { PlaySound(CatchFireSource); };
        SauceSquirt += () => { PlaySound(SauceSquirtSource); };
        FoodFallsInTrash += () => { PlaySound(FoodFallsInTrashSource); };
    }

    private void OnDisable()
    {
        GameOver = null;
        MainTheme = null;
        LooseStar = null;
        ButtonClick = null;
        MoveOrder = null;
        GoodJob = null;
        BellDing = null;
        Chopping = null;
        Grilling = null;
        Sclicing = null;
        FoodDroped = null;
        ToolDropped = null;
        TreadmillSound = null;
        SpawnSqueek = null;
        FoodSpottedSound = null;
        RatLaugh = null;
        DinosaurRoar = null;
        CatchFire = null;
        SauceSquirt = null;
        FoodSpottedSound = null;
    }

    private void PlaySound(AudioSource audio)
    {
        audio.Play();
    }

    private void PlaySound(List<AudioSource> audios)
    {
        int index = Random.Range(0, audios.Count);

        audios[index].Play();
    }
}
