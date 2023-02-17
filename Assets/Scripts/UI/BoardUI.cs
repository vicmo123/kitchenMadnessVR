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


    public void AddOrderToDisplay(Order order)
    {
         CreateOrderUI(order);
    }

    public void RemoveOrder(int id)
    {
        for (int i = ordersUI.Count - 1; i > -1; i--)
        {
            if (id == ordersUI[i].GetId())
            {
                OrderUI orderToDelete = ordersUI[i];
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
}