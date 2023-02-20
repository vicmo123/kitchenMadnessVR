using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BoardUI : MonoBehaviour
{
    private bool DEBUG_MODE = false;

    private const int NB_STARS = 5;
    public Transform orderContainer;
    public RectTransform starContainer;
    public Image star_Prefab;
    public GameObject orderUI_Prefab;
    private Transform orderSlot;

    private List<OrderUI> ordersUI = new List<OrderUI>();
    private List<Transform> stars = new List<Transform>();

    private void Start()
    {
        int nbChild = starContainer.childCount;
        for (int i = 0; i < nbChild; i++)
        {
            Transform star = starContainer.GetChild(i);
            stars.Add(star);
        }
    }

    public void AddOrderToDisplay(Order order)
    {
        CreateOrderUI(order);
    }

    public void RemoveOrderAndCrossOrder(int id)
    {
        for (int i = ordersUI.Count - 1; i > -1; i--)
        {
            if (id == ordersUI[i].GetId())
            {
                OrderUI orderToDelete = ordersUI[i];

                orderToDelete.CrossAppearanceActive();

                ordersUI.Remove(ordersUI[i]);
                GameObject.Destroy(orderToDelete.gameObject);
            }
        }
    }

    public void RemoveOrderAndCelebrate(int id)
    {
        for (int i = ordersUI.Count - 1; i > -1; i--)
        {
            if (id == ordersUI[i].GetId())
            {
                OrderUI orderToDelete = ordersUI[i];

                //Effect Coroutine Disparaitre COmmande
                orderToDelete.RemoveWithJoy();              

                ordersUI.Remove(ordersUI[i]);
                GameObject.Destroy(orderToDelete.gameObject);
            }
        }
    }

    void Update()
    {
        foreach (OrderUI orderUI in ordersUI)
        {
            orderUI.UpdateTimerUI();
        }
    }

    void CreateOrderUI(Order order)
    {
        GameObject go = GameObject.Instantiate<GameObject>(orderUI_Prefab, orderContainer);
        OrderUI newOrder = go.GetComponent<OrderUI>();
        //Link it with the data order
        newOrder.SetOrder(order);
        //newOrder.UpdateTimerUI();
        newOrder.SetIngredientVisible(order.GetRecipe());
        ordersUI.Add(newOrder);
    }
    public void RemoveOneStar()
    {
        if (stars.Count > 1)
        {
            Transform starToDestroy = stars[stars.Count - 1];
            stars.RemoveAt((stars.Count) - 1);
            GameObject.Destroy(starToDestroy.gameObject);
        }
        else if (stars.Count == 1)
        {
            //End Of Game!!!!!
        }
    }
    public void EndOfRound()
    {
        ordersUI.Clear();
    }
}