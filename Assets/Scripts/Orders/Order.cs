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

    EasyIngredient = Sauce,
    MediumIngredient = Onion,
    HardIngredients = Pineapple | Cheese
}
#endregion
public class Order
{
    const float EASY_RECIPE_TIME = 10.0f;
    const float MEDIUM_RECIPE_TIME = 15.0f;
    const float HARD_RECIPE_TIME = 20.0f;
    const float HARDCORE_RECIPE_TIME = 25.0f;
    const float ALMOST_OVER_TIME = 10;
    private static int id = 0;

    public BoardManager board;
    public List<Image> images = new List<Image>();
    private float timeOfCreation;
    private bool isActive = false;
    public float TimeOfCreation { get => timeOfCreation; set => timeOfCreation = value; }
    public bool IsActive { get => isActive; set => isActive = value; }
    public float Pourcentage { get => pourcentage; }

    private OrderTimer orderTimer;
    private IngredientEnum recipe = IngredientEnum.EasyTaco;
    private float pourcentage = 100;
    private bool almostOver = false;
    private int orderId = 0;

   public Order(BoardManager board)
    {
        this.board = board;
        foreach (Image image in images)
        {
            image.rectTransform.gameObject.active = false;
        }
    }
    public void SetOrderTimer()
    {
        Debug.Log("Set Order Timer");
        orderTimer = new OrderTimer(GetRecipeTime());
        orderTimer.timer.OnTimeIsUpLogic = () => { OntimerIsOutLogic(); };
        orderTimer.StartTimer();
        //Load Sprite Prefab of the tortilla and the meat
        //Load Prefab Sprite
    }

    public bool IsCorrespondingToOrder(IngredientEnum ingredients)
    {
        bool corresponding = false;
        if ((recipe | ingredients) == recipe)
        {
            corresponding = true;
        }
        return corresponding;
    }

    public void UpdateOrder()
    {
        orderTimer.UpdateTimer();
        pourcentage = orderTimer.GetPourcentageLeft();
        
        if (pourcentage <= ALMOST_OVER_TIME)
        {
            almostOver = true;
            Debug.Log("Time almost up for order #" + orderId);
        }
    }

    public void SetRecipe(IngredientEnum recipe)
    {
        this.recipe = recipe;
        //Load Sprite Prefabs Accordingly to the list of topings
        
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

        float time;

        if ((int)recipe > 7 & (int)recipe < 30)
        {
            time = MEDIUM_RECIPE_TIME;
        }
        else if ((int)recipe > 30 & (int)recipe < 59)
        {
            time = HARD_RECIPE_TIME;
        }
        else if ((int)recipe >= 60)
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
        Debug.Log("Cross Order");
        //if time is up for the order, the background goes red
        //and the order disapear.
        //TODO        
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

    public void SetId()
    {
        id++;
        orderId = id;
    }

    public int GetId()
    {
        return orderId;
    }

    public void OntimerIsOutLogic()
    {
        CrossOrder();
    }
}