using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const float FIRST_STAGE_GAME = 20;
    private const float SECOND_STAGE_GAME = 30;
    private const float THIRD_STAGE_GAME = 40;

    public BoardUI boardUI;

    private List<Order> activeOrders = new List<Order>();
    private List<Order> iInactiveOrders = new List<Order>();
    const int NB_ORDERS_MAX = 5;
    private float elapsedTime;
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    int currentDifficultyIndex = 0;

    Action RemoveOrder;
    Action BoardEmpty;

    private void Start()
    {
        GenerateOrder();
    }

    public void Update()
    {
       activeOrders.Where(x => x == null).ToList();

       
        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (activeOrders[i] != null)
            {
                activeOrders[i].UpdateOrder();
            }
        }

        //Only one order left on the board, and almost done
        if (activeOrders.Count == 1 && activeOrders[0].IsAlmostOver())
        {
            GenerateOrder();
        }
    }

    public void GenerateOrder()
    {
        if (activeOrders.Count < NB_ORDERS_MAX)
        {
            Order order = new Order(this);
            order.IsActive = true;
            order.SetRecipe(GenerateToppings());
            order.SetOrderTimer();
            order.SetId();

            activeOrders.Add(order);

            Debug.Log("Generate order id : " + order.GetId());
            UpdateUI();
        }
    }

    IngredientEnum GenerateToppings()
    {
        //Default Value
        IngredientEnum ingredients = IngredientEnum.BaseOfTaco;

        //Recipes stored in arrays according to their difficulty level.
        IngredientEnum[] firstRecipe = new IngredientEnum[] { IngredientEnum.BaseOfTaco | IngredientEnum.Sauce };
        IngredientEnum[] easyRecipes = new IngredientEnum[] { IngredientEnum.EasyTaco | IngredientEnum.Sauce, IngredientEnum.EasyTaco };
        IngredientEnum[] mediumRecipes = new IngredientEnum[] { IngredientEnum.EasyTaco | IngredientEnum.Cheese, IngredientEnum.EasyTaco | IngredientEnum.Pineapple };
        IngredientEnum[] hardRecipes = new IngredientEnum[] { IngredientEnum.EasyTaco | IngredientEnum.Cheese | IngredientEnum.Sauce, IngredientEnum.EasyTaco | IngredientEnum.Pineapple | IngredientEnum.Sauce };
        IngredientEnum[] hardCoreRecipes = new IngredientEnum[] { IngredientEnum.HardCoreTaco };
        //Array of Arrays containing all the recipes in one place
        IngredientEnum[][] posssibleRecipes = new IngredientEnum[][] { firstRecipe, easyRecipes, mediumRecipes, hardRecipes, hardCoreRecipes };

        //Levels of difficulty stored in a queue
        float[] timeStages = new float[] { FIRST_STAGE_GAME, SECOND_STAGE_GAME, THIRD_STAGE_GAME };
        Queue<float> stagesGameTime = new Queue<float>(timeStages);

        //Checking which stage of the game we are in 
        //(stages are basically tracking the time spent in the game. The longer we stayed in, the harder it gets)
        if (Time.time >= stagesGameTime.Peek())
        {
            currentDifficultyIndex++;
            stagesGameTime.Dequeue();
        }

        float stage = stagesGameTime.Peek();
        switch (stage)
        {
            case FIRST_STAGE_GAME:
                Debug.Log("First Stage Game");
                ingredients = posssibleRecipes[0][0];
                ingredients = posssibleRecipes[1][0];
                break;
            case SECOND_STAGE_GAME:
                Debug.Log("Second Stage Game");
                ingredients = posssibleRecipes[UnityEngine.Random.Range(1, 4)][UnityEngine.Random.Range(0, 2)];
                break;
            case THIRD_STAGE_GAME:
                Debug.Log("Third Stage Game");
                ingredients = posssibleRecipes[2][0];
                ingredients = posssibleRecipes[3][0];
                ingredients = posssibleRecipes[4][0];
                break;
            default:
                Debug.Log("Generate Toppings, problems");
                break;
        }

        return ingredients;
    }

    public void DoneWithOrder(int id)
    {
        LoseOneStar();
        Debug.Log("Done With Order");

        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (activeOrders[i].IsActive && activeOrders[i].GetId() == id)
            {
                activeOrders[i].IsActive = false;
            }
        }

        for (int i = activeOrders.Count -1; i > -1; i--)
        {
            if(activeOrders[i].IsActive == false)
            {
                activeOrders.Remove(activeOrders[i]);
            }
        }
        //activeOrders.Where(x => x.IsActive == false).ToList();

        UpdateUI();
    }

    int TacoToInt(params IngredientEnum[] recipe)
    {
        IngredientEnum taco = IngredientEnum.Tortilla | IngredientEnum.Meat;

        foreach (IngredientEnum ingredient in recipe)
        {
            taco = taco | ingredient;
        }

        return (int)taco;
    }

    void UpdateUI()
    {
        if (activeOrders != null)
            boardUI.UpdateOrdersToDisplay(activeOrders);
    }

    public void LoseOneStar()
    {
        //TODO
        Debug.Log("Lose one star, oh no!");
    }
}