using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#region Ingredients
public enum Ingredients
{
    //sauce | cheese | pineapple | onion | meat | tortilla
    // exemple : 100011 = taco that constains sauce + meat + tortilla

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
#endregion
public class Order : MonoBehaviour
{
    public BoardManager board;
    public GameObject orderTimerPrefab;
    private OrderTimer orderTimer;
    private int orderIngredients = 0b111000;
    private bool isInUse = false;

    private void Start()
    {
        GameObject go = GameObject.Instantiate(orderTimerPrefab, this.transform);
        orderTimer = go.GetComponent<OrderTimer>();
        orderTimer.SetOrder(this);
        orderTimer.TimerIstOut += () => { CrossOrder(); };
        SetOrderTimer(5);
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

    public void ComputeIngredientsInTime()
    {
        //sauce | cheese | pineapple | onion | meat | tortilla
        // exemple : 100011 = taco that constains sauce + meat + tortilla
        // tortilla + meat = 3
        // tortilla + meat + onion = 7
        // tortilla + meat + onion + sauce = 39
        // tortilla + meat + onion + pineapple = 15
        // tortilla + meat + onion + pineapple + sauce  = 47
        // tortilla + meat + onion + cheese + sauce  = 55
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