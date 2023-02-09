using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#region Ingredients
public enum Ingredients
{
    //pineapple | cheese | onion | sauce | meat | tortilla
    // exemple : 100011 = taco that constains pineappl + meat + tortilla

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
public class Order : MonoBehaviour
{
    public BoardManager board;
    public GameObject orderTimerPrefab;
    private OrderTimer orderTimer;
    private int orderIngredients = 0b111000;
    private bool isInUse = false;

    const float EASY_RECIPE_TIME = 60.0f;
    const float MEDIUM_RECIPE_TIME = 90.0f;
    const float HARD_RECIPE_TIME = 120.0f;
    const float HARDCORE_RECIPE_TIME = 140.0f;

    private void Start()
    {
        GameObject go = GameObject.Instantiate(orderTimerPrefab, this.transform);
        orderTimer = go.GetComponent<OrderTimer>();
        orderTimer.SetOrder(this);
        orderTimer.TimerIstOut += () => { CrossOrder(); };
        SetRecipeTime();
        //Load Sprite Prefab of the tortilla and the meat
        //Load Prefab Sprite
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

    }

    public void SetTacoIngredients(int ingredientsSerialized)
    {
        orderIngredients = ingredientsSerialized;
        //Load Sprite Prefabs Accordingly to the list of topings
    }

    public void SetOrderTimer(float duration)
    {
        orderTimer.SetDuration(duration);
    }

    public void SetRecipeTime()
    {
        //sauce | cheese | pineapple | onion | meat | tortilla
        // exemple : 100011 = taco that constains sauce + meat + tortilla
        // tortilla + meat = 3
        // tortilla + meat + sauce = 7
        // tortilla + meat + onion + sauce = 15
        // tortilla + meat + onion + pineapple = 43
        // tortilla + meat + onion + pineapple + sauce  = 47
        // tortilla + meat + onion + cheese + sauce  = 31      

        if (orderIngredients > 7 & orderIngredients < 30)
        {
            SetOrderTimer(MEDIUM_RECIPE_TIME);
        }
        else if (orderIngredients > 30 & orderIngredients < 59)
        {
            SetOrderTimer(HARD_RECIPE_TIME);
        }
        else if (orderIngredients >= 60)
        {
            SetOrderTimer(HARDCORE_RECIPE_TIME);
        }
        else
        {
            SetOrderTimer(EASY_RECIPE_TIME);
        }

    }

    public void CrossOrder()
    {
        //if time is up for the order, the background goes red
        //and the order disapear.
        //TODO
        Debug.Log("Taco : " + orderIngredients + " = Order crossed cause Timer went out!");
        board.LoseOneStar();
        board.DoneWithOrder(this);
        Reset();
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
    }

}