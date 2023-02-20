using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private bool DEBUG_MODE = true;

    private const float FIRST_STAGE_GAME = 60;
    private const float SECOND_STAGE_GAME = 90;
    private const float THIRD_STAGE_GAME = 180;
    private const int NB_ORDERS_MAX = 5;
    private int currentNbStars = 5;

    public BoardUI boardUI;
    public StarManager starManager;

    private List<Order> activeOrders = new List<Order>();
    private float elapsedTime;
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    int currentDifficultyIndex = 0;

    public bool roundActive { get; set; } = false;

    private void Start()
    {
        // GenerateOrder();
    }

    public void Update()
    {
        //activeOrders.Where(x => x == null).ToList();
        if (roundActive)
        {
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
            else if (activeOrders.Count == 0)
            {
                GenerateOrder();
            }
        }
        else
        {
            EndOfRound();
        }
    }

    public void GenerateOrder()
    {
        if (activeOrders.Count < NB_ORDERS_MAX)
        {
            Order order = new Order(this, GenerateToppings());
            activeOrders.Add(order);
            if (DEBUG_MODE)
                Debug.Log("Generate order id : " + order.GetId());

            if (DEBUG_MODE)
            {
                foreach (Order item in activeOrders)
                {
                    Debug.Log("Order in the active list : " + item.GetId());
                }
            }
            boardUI.AddOrderToDisplay(order);
        }
    }

    IngredientEnum GenerateToppings()
    {
        //Default Value
        IngredientEnum ingredients = IngredientEnum.BaseOfTaco;

        //Recipes stored in arrays according to their difficulty level.
        IngredientEnum[] firstRecipe = new IngredientEnum[] { IngredientEnum.BaseOfTaco | IngredientEnum.Sauce };
        IngredientEnum[] easyRecipes = new IngredientEnum[] { IngredientEnum.EasyTaco | IngredientEnum.Sauce, IngredientEnum.BaseOfTaco };
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
                if (DEBUG_MODE)
                    Debug.Log("First Stage Game");

                if (Time.time % 2 == 0)
                    ingredients = posssibleRecipes[0][0];
                else
                    ingredients = posssibleRecipes[1][UnityEngine.Random.Range(0, 2)];
                break;
            case SECOND_STAGE_GAME:
                if (DEBUG_MODE)
                    Debug.Log("Second Stage Game");

                ingredients = posssibleRecipes[UnityEngine.Random.Range(1, 4)][UnityEngine.Random.Range(0, 2)];
                break;
            case THIRD_STAGE_GAME:
                if (DEBUG_MODE)
                    Debug.Log("Third Stage Game");

                if (Time.time % 2 == 0)
                    ingredients = posssibleRecipes[2][UnityEngine.Random.Range(0, 2)];
                else if (Time.time % 3 == 0)
                    ingredients = posssibleRecipes[4][0];
                else
                    ingredients = posssibleRecipes[3][UnityEngine.Random.Range(0, 2)];
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
        SoundManager.LooseStar?.Invoke();

        if (DEBUG_MODE)
            Debug.Log("Done With Order");

        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (activeOrders[i].IsActive && activeOrders[i].GetId() == id)
            {
                activeOrders[i].IsActive = false;
            }
        }

        for (int i = activeOrders.Count - 1; i > -1; i--)
        {
            if (activeOrders[i].IsActive == false)
            {
                activeOrders.Remove(activeOrders[i]);
            }
        }

        boardUI.RemoveOrderAndCrossOrder(id);

        if (DEBUG_MODE)
        {
            if (activeOrders.Count == 0)
            {
                Debug.Log("List of active orders is empty.");
            }
            else
            {
                foreach (Order item in activeOrders)
                {
                    Debug.Log("Order in the active list : " + item.GetId());
                }
            }
        }
    }

    public void LoseOneStar()
    {
        boardUI.RemoveOneStar(currentNbStars);
        starManager.Current_nb_stars--;
        currentNbStars--;
    }

    public bool isTacoGoodToServe(IngredientEnum taco)
    {
        bool result = false;
        int indexRecipeToRemove = -1;
        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (activeOrders[i].IsCorrespondingToOrder(taco))
            {
                if (DEBUG_MODE)
                    Debug.Log("Order Corresponding with taco!");
                result = true;
                indexRecipeToRemove = i;
                break;
            }
        }
        if (indexRecipeToRemove != -1)
        {
            Order order = activeOrders[indexRecipeToRemove];
            activeOrders.Remove(activeOrders[indexRecipeToRemove]);
            boardUI.RemoveOrderAndCelebrate(order.GetId());
        }
        return result;
    }

    private void EndOfRound()
    {
        boardUI.EndOfRound();
        activeOrders.Clear();
    }

    public int GetCurrentNbStars()
    {
        return currentNbStars;
    }

}