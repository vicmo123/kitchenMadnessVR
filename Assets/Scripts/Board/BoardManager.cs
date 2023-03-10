using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private bool DEBUG_MODE = false;

    private const float FIRST_STAGE_GAME = 1; // time spent in the game in seconds
    private const float SECOND_STAGE_GAME = 300; // time spent in the game in seconds
    private const float THIRD_STAGE_GAME = 600; // time spent in the game in seconds
    private const int NB_ORDERS_MAX = 5;
    private int currentNbStars = 5;
    private int nbRounds = 0;
    private float lastOrderStampTime = 0;
    private bool newRound = false;
    private int nbTacosServed = 0;

    public BoardUI boardUI;
    public StarManager starManager;

    private List<Order> activeOrders = new List<Order>();
    private float elapsedTime;
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
    int currentDifficultyIndex = 0;

    public bool roundActive { get; set; } = false;
    public int NbTacosServed { get => nbTacosServed; set => nbTacosServed = value; }
    public bool NewRound { get => newRound; set => newRound = value; }

    #region Recipes lists
    //These arrays are initialised in the Start function
    IngredientEnum[] firstRecipe = new IngredientEnum[1];
    IngredientEnum[] easyRecipes = new IngredientEnum[2];
    IngredientEnum[] mediumRecipes = new IngredientEnum[2];
    IngredientEnum[] hardRecipes = new IngredientEnum[3];
    IngredientEnum[] hardCoreRecipes = new IngredientEnum[1];
    IngredientEnum[][] posssibleRecipes = new IngredientEnum[5][];
    #endregion

    private void Start()
    {
        //Recipes stored in arrays according to their difficulty level.
        firstRecipe[0] = IngredientEnum.EasyTaco;

        easyRecipes[0] = IngredientEnum.EasyTaco;
        easyRecipes[1] = IngredientEnum.BaseOfTaco;

        mediumRecipes[0] = IngredientEnum.BaseOfTaco | IngredientEnum.Onion;
        mediumRecipes[1] = IngredientEnum.EasyTaco | IngredientEnum.Onion;

        hardRecipes[0] = IngredientEnum.BaseOfTaco | IngredientEnum.Onion | IngredientEnum.Cheese;
        hardRecipes[1] = IngredientEnum.BaseOfTaco | IngredientEnum.Onion | IngredientEnum.Pineapple;
        hardRecipes[2] = IngredientEnum.BaseOfTaco | IngredientEnum.Cheese | IngredientEnum.Pineapple;

        hardCoreRecipes[0] = IngredientEnum.HardCoreTaco;

        //Array of Arrays containing all the recipes in one place
        posssibleRecipes[0] = firstRecipe;
        posssibleRecipes[1] = easyRecipes;
        posssibleRecipes[2] = mediumRecipes;
        posssibleRecipes[3] = hardRecipes;
        posssibleRecipes[4] = hardCoreRecipes;
    }

    public void Update()
    {
        if (roundActive)
        {
            NewRound = true;
            boardUI.SetVisibleEndRoundMessage(false);

            if (activeOrders.Count > 0)
            {
                for (int i = 0; i < activeOrders.Count; i++)
                {
                    if (activeOrders[i] != null)
                    {
                        activeOrders[i].UpdateOrder();
                    }
                }
                if (activeOrders.Count == 1 && activeOrders[0].IsAlmostOver())
                {
                    GenerateOrder();
                }
                else if (activeOrders.Count < NB_ORDERS_MAX && (elapsedTime - lastOrderStampTime >= 60))
                {
                    GenerateOrder();

                }
            }
            else
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

            boardUI.AddOrderToDisplay(order);
            lastOrderStampTime = elapsedTime;
        }
    }

    IngredientEnum GenerateToppings()
    {
        //Default recipe generated automatically at the beginning of each round.
        IngredientEnum ingredients = IngredientEnum.BaseOfTaco;

        //Levels of difficulty stored in array
        float[] timeStages = new float[] { FIRST_STAGE_GAME, SECOND_STAGE_GAME, THIRD_STAGE_GAME };

        //Checking which stage of the game we are in 
        //(stages are basically tracking the time spent in the game. The longer we stay in, the harder it gets, time is reset on each round)
        if (currentDifficultyIndex + 1 < timeStages.Length && elapsedTime >= timeStages[currentDifficultyIndex + 1])
        {
            currentDifficultyIndex++;
        }
        if (DEBUG_MODE)
            Debug.Log("ElapsedTime : " + elapsedTime);

        float stage = timeStages[currentDifficultyIndex];
        switch (stage)
        {
            case FIRST_STAGE_GAME:
                if (DEBUG_MODE)
                    Debug.Log("First Stage Game");
                //Random between arrays of difficulty, and what they contain (recipes).
                int index = UnityEngine.Random.Range(0, 2);
                ingredients = posssibleRecipes[index][posssibleRecipes[index].GetRandomElement<IngredientEnum>()];

                break;

            case SECOND_STAGE_GAME:
                if (DEBUG_MODE)
                    Debug.Log("Second Stage Game");

                int index1 = UnityEngine.Random.Range(1, 4);
                ingredients = posssibleRecipes[index1][UnityEngine.Random.Range(0, posssibleRecipes[index1].Length)];
                break;

            case THIRD_STAGE_GAME:
                if (DEBUG_MODE)
                    Debug.Log("Third Stage Game");

                int index2 = UnityEngine.Random.Range(2, 5);
                ingredients = posssibleRecipes[index2][UnityEngine.Random.Range(0, posssibleRecipes[index2].Length)];
                break;

            default:
                Debug.Log("Generate Toppings, problems");
                break;
        }

        return ingredients;
    }

    public void DoneWithOrder(int id)
    {

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

        LoseOneStar();
        SoundManager.LooseStar?.Invoke();       
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
        if (newRound)
        {
            if (DEBUG_MODE)
            {
                Debug.Log("END OF ROUND!!!!!!!!!!!!!!!");
            }
            boardUI.EndOfRound(nbTacosServed);
            activeOrders.Clear();
            currentNbStars = 5;
            currentDifficultyIndex = 0;
            lastOrderStampTime = 0;
            elapsedTime = 0;
        }
    }

    public int GetCurrentNbStars()
    {
        return currentNbStars;
    }
}