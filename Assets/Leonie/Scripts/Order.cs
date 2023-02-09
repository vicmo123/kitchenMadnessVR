using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Ingredients
{
    //tortilla | meat | onion | pineapple | cheese | sauce
    None = 0,	//must have a specified 0
    Tortilla = 1 << 0,	//1
    Meat = 1 << 1,	//2
    Onion = 1 << 2,	//4
    Pineapple = 1 << 3,	//8
    Cheese = 1 << 4,	//16
    Sauce = 1 << 5, //32  

    BaseOfTaco = Tortilla | Meat,
    EasyTaco = Tortilla | Meat | Onion,
    HardCoreTaco = Tortilla | Meat | Onion | Pineapple | Cheese | Sauce,

    EasyIngredient = Sauce,
    MediumIngredient = Onion,
    HardIngredients = Pineapple | Cheese
}
public class Order : MonoBehaviour
{
    public BoardManager board;
    private int orderIngredients = 0b111000;
    //private OrderTimer orderTimer;
    private bool isInUse = false;
    private bool timerIsUp = false;
    public bool TimerIsUp { get => timerIsUp; set => timerIsUp = value; }

    private void Start()
    {       
        //Load Prefab of the tortilla and the meat
        //Load Prefab Sprite
       // orderTimer = new OrderTimer();
        //OrderLostEvent += board.DoneWithOrder;
    }

    public bool IsCorrespondingToOrder(int ingredients)
    {
        bool corresponding = false;
        if ((orderIngredients | ingredients) == orderIngredients)
        {
            corresponding = true;
        }
        return corresponding;
    }

    private void Update()
    {
        if (TimerIsUp)
        {
            CrossOrder();
            board.LoseOneStar();
            board.DoneWithOrder(this);
            Reset();
        }
    }

    public void SetTacoIngredients(int ingredientsSerialized)
    {
        orderIngredients = ingredientsSerialized;
        //Load Sprite Prefabs Accordingly to the list of topings
    }

    public void SetOrderTimer(float duration)
    {
       // orderTimer.SetDuration(duration);
    }   

    public void CrossOrder()
    {
       //if time is up for the order, the background goes red
       //and the order disapear.
        //TODO
    }

    public void SetInUse(bool inUse)
    {
        isInUse = inUse;
    }

    public bool GetIsInUse()
    {
        return isInUse;
    }


    Ingredients AddIngredientToTaco(Ingredients taco, Ingredients topingToAdd)
    {
        return taco | topingToAdd;
    }

    private void Reset()
    {
        TimerIsUp = false;
    }
}
