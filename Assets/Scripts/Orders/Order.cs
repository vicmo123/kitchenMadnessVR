using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#region Ingredients
public enum IngredientEnum
{
    //pineapple | cheese | onion | sauce | meat | tortilla
    // exemple : 100011 = taco that constains pineapple + meat + tortilla

    None = 0,	//must have a specified 0
    Tortilla = 1 << 0,	//1
    Meat = 1 << 1,	//2
    Sauce = 1 << 2,	//4
    Onion = 1 << 3,	//8
    Cheese = 1 << 4,	//16
    Pineapple = 1 << 5, //32  

    BaseOfTaco = Tortilla | Meat,
    EasyTaco = Tortilla | Meat | Sauce,
    HardCoreTaco = Tortilla | Meat | Onion | Pineapple | Cheese | Sauce,
}
#endregion
public class Order
{
    private bool DEBUG_MODE = false;

    const float EASY_RECIPE_TIME = 180.0f;
    const float MEDIUM_RECIPE_TIME = 200.0f;
    const float HARD_RECIPE_TIME = 240.0f;
    const float HARDCORE_RECIPE_TIME = 280.0f;
    const float ALMOST_OVER_TIME = 20;
    private static int id = 0;

    public BoardManager board;
    private bool isActive = false;
    public bool IsActive { get => isActive; set => isActive = value; }
    public float PourcentageLeft { get => pourcentage; set => pourcentage = value; }

    //private OrderTimer orderTimer;
    private IngredientEnum recipe = IngredientEnum.EasyTaco;
    private bool almostOver = false;
    private int orderId = 0;
    private float duration;
    private float timeIsOver;
    private float startTime;
    
    private float pourcentage;

    public Order(BoardManager board, IngredientEnum recipe)
    {
        this.board = board;
        this.IsActive = true;
        this.recipe = recipe;
        orderId = ++id;
        SetOrderTimer();
    }

    public void SetOrderTimer()
    {
        startTime = Time.time;
        duration = GetRecipeTime();
        timeIsOver = duration + startTime;
    }

    public bool IsCorrespondingToOrder(IngredientEnum ingredients)
    {
        bool corresponding = false;
        if (ingredients == recipe)
        {
            corresponding = true;
        }
        return corresponding;
    }

    public void UpdateOrder()
    {
        PourcentageLeft = 1 - ((Time.time - startTime) / (timeIsOver - startTime));

        if (PourcentageLeft < .3f && almostOver == false)
        {
            almostOver = true;
            if (DEBUG_MODE)
                Debug.Log("Time Almost Over :  30%");
        }
        else if (timeIsOver <= Time.time)
        {
            CrossOrder();
        }
    }

    public void SetRecipe(IngredientEnum recipe)
    {
        this.recipe = recipe;
    }

    public IngredientEnum GetRecipe()
    {
        return recipe;
    }

    public float GetRecipeTime()
    {
        //sauce | cheese | pineapple | onion | meat | tortilla
        // exemple : 100011 = taco that constains sauce + meat + tortilla
        // tortilla + meat = 3
        // tortilla + meat + sauce = 7
        // tortilla + meat + onion + sauce = 15
        // tortilla + meat + onion + cheese + sauce  = 31      
        // tortilla + meat + onion + pineapple = 43
        // tortilla + meat + onion + pineapple + sauce  = 47
        int value = (int)this.recipe;
        float time;

        if (value > 7 & value < 30)
        {
            time = MEDIUM_RECIPE_TIME;
        }
        else if (value > 30 & value < 59)
        {
            time = HARD_RECIPE_TIME;
        }
        else if (value >= 60)
        {
            time = HARDCORE_RECIPE_TIME;
        }
        else
        {
            time = EASY_RECIPE_TIME;
        }

        return time;
    }

    public void CrossOrder()
    {
        board.DoneWithOrder(orderId);
    }


    IngredientEnum AddIngredientToTaco(IngredientEnum taco, IngredientEnum topingToAdd)
    {
        return taco | topingToAdd;
    }

    public bool IsAlmostOver()
    {
        return almostOver;
    }

    public int GetId()
    {
        return orderId;
    }
}