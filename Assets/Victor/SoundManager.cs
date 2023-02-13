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
    //Rats
    public static Action SpawnSqueek;
    public static Action FoodSpottedSound;
    //Dinosaurs
    public static Action DinosaurRoar;

    //Audio Source
    public AudioSource GameOverSource;
    public AudioSource MainThemeSource;
    public AudioSource LooseStarSource;
    public AudioSource ButtonClickSource;
    public AudioSource MoveOrderSource;
    public AudioSource GoodJobSource;
    public AudioSource BellDingSource;
    public AudioSource ChoppingSource;
    public AudioSource GrillingSource;
    public AudioSource SclicingSource;
    public AudioSource FoodDropedSource;
    public AudioSource ToolDroppedSource;
    public AudioSource TreadmillSoundSource;
    public AudioSource SpawnSqueekSource;
    public AudioSource FoodSpottedSoundSource;
    public AudioSource DinosaurRoarSource;

    public SoundData Data;

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
        DinosaurRoarSource = Instantiate(Data.DinosaurRoar, transform);


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
        DinosaurRoar += () => { PlaySound(DinosaurRoarSource); };
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
        DinosaurRoar = null;
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
