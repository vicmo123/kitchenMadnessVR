using System;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    private const int FIRST_STAGE_GAME = 60;
    private const int SECOND_STAGE_GAME = 240;
    private const int THIRD_STAGE_GAME = 420;
    //public Player player
    public BoardUI boardUI;
    public GameObject prefabOrder;

    private List<Order> orders = new List<Order>();
    const int NB_ORDERS_LIMIT = 5;
    int nbOrdersDisplayed = 1;
    private float elapsedTime;
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }

    Action RemoveOrder;


    private void Awake()
    {
        InitialiseList();
    }


    void InitialiseList()
    {
        SetFirstTaco();

        for (int i = 1; i < NB_ORDERS_LIMIT; i++)
        {
            GameObject go = GameObject.Instantiate(prefabOrder, this.transform);
            Order order = go.GetComponent<Order>();
            order.SetInUse(false);
            order = GenerateToppings(order);
            orders.Add(order);
        }

        UpdateUI();
    }


    private void Update()
    {
        //Order order = GenerateOrder();
        //order.SetInUse(true);
    }

    public void GenerateOrder(int index)
    {
        Order order = orders[index];
        order = GenerateToppings(order);
        order.SetInUse(true);
        order.TimeOfCreation = ElapsedTime;

        nbOrdersDisplayed++;
        UpdateUI();

    }

    Order GenerateToppings(Order order)
    {
        int ingredients = (int)Ingredients.EasyTaco;
        int result = UnityEngine.Random.Range(0, 2);

        //(first 1 min ) 
        order.SetTacoIngredients((int)(Ingredients.BaseOfTaco | Ingredients.Sauce));

        // ( from 1 min to 4 minutes in the game )        
        if (elapsedTime > FIRST_STAGE_GAME & elapsedTime < SECOND_STAGE_GAME)
        {
            ingredients = (int)GetEasyRecipe();
        }

        //( from 4 to 7 minutes in the game )
        else if (elapsedTime > SECOND_STAGE_GAME & elapsedTime < THIRD_STAGE_GAME)
        {
            if (elapsedTime % 2 == 0)
            {
                ingredients = (int)GetMediumRecipe();
            }
            else if (elapsedTime % 3 == 0)
            {
                ingredients = (int)GetHardRecipe();
            }
            else
            {
                ingredients = (int)GetEasyRecipe();
            }
        }
        //(after 7 minutes)
        else if (elapsedTime > THIRD_STAGE_GAME)
        {
            if (elapsedTime % 2 == 0)
            {
                ingredients = (int)GetHardRecipe();
            }
            else if (elapsedTime % 3 == 0)
            {
                ingredients = (int)Ingredients.HardCoreTaco;
            }
            else if (elapsedTime % 4 == 0)
            {
                ingredients = (int)GetMediumRecipe();
            }
            else
            {
                ingredients = (int)GetEasyRecipe();
            }
        }

        order.SetTacoIngredients(ingredients);

        return order;
    }

    private Ingredients GetEasyRecipe()
    {
        Ingredients ingredients = Ingredients.EasyTaco | Ingredients.Sauce;

        if (elapsedTime % 2 == 0)
        {
            ingredients = Ingredients.EasyTaco;
        }

        return ingredients;
    }

    private Ingredients GetMediumRecipe()
    {
        Ingredients ingredients = Ingredients.EasyTaco | Ingredients.Cheese;

        if (elapsedTime % 2 == 0)
        {
            ingredients = Ingredients.EasyTaco | Ingredients.Pineapple;
        }
        return ingredients;
    }

    private Ingredients GetHardRecipe()
    {
        Ingredients ingredients = Ingredients.EasyTaco | Ingredients.Cheese | Ingredients.Sauce;

        if (elapsedTime % 2 == 0)
        {
            ingredients = Ingredients.EasyTaco | Ingredients.Pineapple | Ingredients.Sauce;
        }
        return ingredients;
    }

    void SetFirstTaco()
    {
        GameObject order = GameObject.Instantiate(prefabOrder, this.transform);
        Order firstOrder = order.GetComponent<Order>();

        firstOrder.SetTacoIngredients((int)Ingredients.EasyTaco);
        firstOrder.SetInUse(true);
        firstOrder.TimeOfCreation = ElapsedTime;

        orders.Add(firstOrder);
    }

    public void DoneWithOrder(Order order)
    {
        order.SetInUse(false);
        nbOrdersDisplayed--;
        UpdateUI();
    }

    int TacoToInt(params Ingredients[] ingredients)
    {
        Ingredients taco = Ingredients.Tortilla | Ingredients.Meat;

        foreach (Ingredients ingredient in ingredients)
        {
            taco = taco | ingredient;
        }

        return (int)taco;
    }

    void UpdateUI()
    {
        List<Order> temp = new List<Order>();

        //transfering only the active orders
        foreach (Order order in orders)
        {
            if (order.GetIsInUse())
            {
                temp.Add(order);
            }
        }
        boardUI.UpdateOrdersToDisplay(temp);
    }

    public void LoseOneStar()
    {
        //TODO
        Debug.Log("Lose one star, oh no!");
    }
}
