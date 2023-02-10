using System;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour
{
    private const int FIRST_STAGE_GAME = 60;
    private const int SECOND_STAGE_GAME = 240;
    private const int THIRD_STAGE_GAME = 420;

    public BoardUI boardUI;
    public GameObject prefabOrder;
    private Timer timer;
    private List<Order> orders = new List<Order>();
    const int NB_ORDERS_LIMIT = 5;
    int nbActiveOrder = 1;
    private float elapsedTime;
    public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }

    Action RemoveOrder;
    Action BoardEmpty;


    private void Awake()
    {
        timer = new Timer();
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
            order.TimeOfCreation = Time.time + i;
            orders.Add(order);
        }

        UpdateUI();
    }


    public void Update()        
    {
        elapsedTime = Time.time;
        //Only one order left on the board, and almost done
        if (nbActiveOrder == 1 && orders[0].IsAlmostOver())
        {
            Debug.Log("Timer almost Up on order at index : " + 0);
            GenerateOrder();
        }
    }

    public void GenerateOrder()
    {
        Debug.Log("Generating new order at index : " + nbActiveOrder);
        orders[nbActiveOrder] = GenerateToppings(orders[nbActiveOrder]);
        orders[nbActiveOrder].SetRecipeTime();
        orders[nbActiveOrder].SetInUse(true);
        orders[nbActiveOrder].TimeOfCreation = Time.time;
        Debug.Log("Time of creation for order at index : " + nbActiveOrder + " :" + orders[nbActiveOrder].TimeOfCreation);

        nbActiveOrder++;
        UpdateUI();

    }

    Order GenerateToppings(Order order)
    {
        //Default Value
        int ingredients = (int)IngredientEnum.EasyTaco;

        //(first 1 min ) 
        if (elapsedTime < FIRST_STAGE_GAME)
        {
            order.SetTacoIngredients((int)(IngredientEnum.BaseOfTaco | IngredientEnum.Sauce));
        }

        // ( from 1 min to 4 minutes in the game )        
        else if (elapsedTime > FIRST_STAGE_GAME & elapsedTime < SECOND_STAGE_GAME)
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
                ingredients = (int)IngredientEnum.HardCoreTaco;
            }
            else if (elapsedTime % 5 == 0)
            {
                ingredients = (int)GetMediumRecipe();
            }
            else
            {
                ingredients = (int)GetEasyRecipe();
            }
        }

        order.SetTacoIngredients(ingredients);
        Debug.Log("Taco ingredients : " + ingredients);

        return order;
    }

    private IngredientEnum GetEasyRecipe()
    {
        IngredientEnum ingredients = IngredientEnum.EasyTaco | IngredientEnum.Sauce;

        if (elapsedTime % 2 == 0)
        {
            ingredients = IngredientEnum.EasyTaco;
        }

        return ingredients;
    }

    private IngredientEnum GetMediumRecipe()
    {
        IngredientEnum ingredients = IngredientEnum.EasyTaco | IngredientEnum.Cheese;

        if (elapsedTime % 2 == 0)
        {
            ingredients = IngredientEnum.EasyTaco | IngredientEnum.Pineapple;
        }
        return ingredients;
    }

    private IngredientEnum GetHardRecipe()
    {
        IngredientEnum ingredients = IngredientEnum.EasyTaco | IngredientEnum.Cheese | IngredientEnum.Sauce;

        if (elapsedTime % 2 == 0)
        {
            ingredients = IngredientEnum.EasyTaco | IngredientEnum.Pineapple | IngredientEnum.Sauce;
        }
        return ingredients;
    }

    void SetFirstTaco()
    {
        GameObject order = GameObject.Instantiate(prefabOrder, this.transform);
        Order firstOrder = order.GetComponent<Order>();

        firstOrder.SetTacoIngredients((int)IngredientEnum.EasyTaco);
        firstOrder.SetInUse(true);
        firstOrder.TimeOfCreation = Time.time;

        orders.Add(firstOrder);
    }

    public void DoneWithOrder(Order order)
    {
        //find the index of this order in the list
        int index = 0;
        
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i].TimeOfCreation == order.TimeOfCreation)
            {
                index = i;
                Debug.Log("DoneWithOrder function, found order index : " + index);
                break;
            }
        }

        //reorder the list 
        while (index + 1 < orders.Count && orders[index + 1].GetIsInUse())
        {
            Swap<Order>(orders, index, index + 1);
            index = index ++;
        }

        for (int i = 0; i < orders.Count; i++)
        {
            Debug.Log("List ordered :" + i + " item active : " + orders[i].GetIsInUse() + "time " + orders[i].TimeOfCreation);
        }
       
        nbActiveOrder--;
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

    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
}
