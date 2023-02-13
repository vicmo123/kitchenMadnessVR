using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundData", menuName = "SoundsData/SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    //Main
    public AudioSource GameOver;
    public AudioSource MainTheme;
    public AudioSource LooseStar;
    public AudioSource ButtonClick;
    public AudioSource MoveOrder;
    public AudioSource GoodJob;

    //Cooking
    public AudioSource BellDing;
    public AudioSource Chopping;
    public AudioSource Grilling;
    public AudioSource Sclicing;
    public AudioSource FoodDroped;
    public AudioSource ToolDropped;
    public AudioSource TreadmillSound;

    //Rats
    public AudioSource SpawnSqueek;
    public AudioSource FoodSpottedSound;

    //Dinosaurs
    public AudioSource DinosaurRoar;
}
