using System;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    //public Player player
    public BoardUI boardUI;
    public GameObject prefabOrder;
    private List<Order> orders = new List<Order>();
    const int nbOrdersLimit = 5;

    Action RemoveOrder;
    int nbOrdersDisplayed = 1;

    private void Awake()
    {
        InitialiseList();
    }


    void InitialiseList()
    {
        SetFirstTaco();

        for (int i = 1; i < nbOrdersLimit; i++)
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

        nbOrdersDisplayed++;
        UpdateUI();

    }

    Order GenerateToppings(Order order)
    {
        int timerSeconds = 60;
        int ingredients;
        int result = UnityEngine.Random.Range(0, 2);

        //STATE BEGIN   (first 1 min )     
        order.SetTacoIngredients((int)(Ingredients.BaseOfTaco | Ingredients.Sauce));

        //STATE GETTING TO KNOW THE ENVIRONMENT ( from 1 min to 4 minutes in the game )        
        if (result == 0)
        {
            ingredients = (int)Ingredients.EasyTaco;
        }
        else
        {
            ingredients = (int)(Ingredients.EasyTaco | Ingredients.Sauce);
        }

        //STATE READY FOR A LITTLE CHALLENGE ( from 4 to 7 minutes in the game )        
        if (result == 0)
        {
            ingredients = (int)(Ingredients.EasyTaco | Ingredients.Pineapple);
        }
        else
        {
            ingredients = (int)(Ingredients.EasyTaco | Ingredients.Cheese);

        }
        //STATE READY FOR EPIC FUN (after 7 minutes)
        ingredients = (int)Ingredients.HardCoreTaco;



        order.SetTacoIngredients(ingredients);
        //Set Timer corresponding to the difficulty of the ingredients chosen
        // order.SetOrderTimer(timerSeconds);

        return order;
    }
    
    void SetFirstTaco()
    {
        GameObject order = GameObject.Instantiate(prefabOrder, this.transform);
        Order firstOrder = order.GetComponent<Order>();

        firstOrder.SetTacoIngredients((int)Ingredients.EasyTaco);
        firstOrder.SetInUse(true);

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
