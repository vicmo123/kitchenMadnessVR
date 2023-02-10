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
public class Order : MonoBehaviour
{
    const float EASY_RECIPE_TIME = 6.0f;
    const float MEDIUM_RECIPE_TIME = 8.0f;
    const float HARD_RECIPE_TIME = 10.0f;
    const float HARDCORE_RECIPE_TIME = 12.0f;
    const float ALMOST_OVER_TIME = 2;

    public BoardManager board;
    public GameObject orderTimerPrefab;
    private float timeOfCreation;
    public float TimeOfCreation { get => timeOfCreation; set => timeOfCreation = value; }
    private OrderTimer orderTimer;
    private int recipe = 0b111000;
    private bool isInUse = false;
    private bool almostOver = false;

    private void Awake()
    {
        GameObject go = GameObject.Instantiate(orderTimerPrefab, this.transform);
        orderTimer = go.GetComponent<OrderTimer>();
    }

    private void Start()
    {
        
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
        if ((recipe | ingredients) == recipe)
        {
            corresponding = true;
        }
        return corresponding;
    }

    private void Update()
    {
        float timeLeft = orderTimer.GetTimeLeft();
        if(timeLeft <= ALMOST_OVER_TIME)
        {
            almostOver = true;
        }
    }

    public void SetTacoIngredients(int ingredientsSerialized)
    {
        recipe = ingredientsSerialized;
        //Load Sprite Prefabs Accordingly to the list of topings
    }

    public void SetOrderTimer(float duration)
    {
        orderTimer.SetDuration(duration);
        orderTimer.StartTimer();
    }

    public void SetRecipeTime()
    {
        //sauce | cheese | pineapple | onion | meat | tortilla
        // exemple : 100011 = taco that constains sauce + meat + tortilla
        // tortilla + meat = 3
        // tortilla + meat + sauce = 7
        // tortilla + meat + onion + sauce = 15
        // tortilla + meat + onion + cheese + sauce  = 31      
        // tortilla + meat + onion + pineapple = 43
        // tortilla + meat + onion + pineapple + sauce  = 47

        if (recipe > 7 & recipe < 30)
        {
            SetOrderTimer(MEDIUM_RECIPE_TIME);
        }
        else if (recipe > 30 & recipe < 59)
        {
            SetOrderTimer(HARD_RECIPE_TIME);
        }
        else if (recipe >= 60)
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
        Debug.Log("Taco : " + recipe + " = Order crossed cause Timer went out!");
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


    IngredientEnum AddIngredientToTaco(IngredientEnum taco, IngredientEnum topingToAdd)
    {
        return taco | topingToAdd;
    }

    public void Reset()
    {
        //TODO
        isInUse = false;
        almostOver = false;        
    }

    public bool IsAlmostOver()
    {
        return almostOver;
    }

}