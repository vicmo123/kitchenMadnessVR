using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BoardUI : MonoBehaviour
{
    public List<Image> images = new List<Image>();
    private List<Order> orders = new List<Order>();

    private void DisplayOrders()
    {
        //TODO
        //display orders on the board in a grid layout
    }
    public void UpdateOrdersToDisplay(List<Order> updatedList)
    {
        orders.Clear();
        foreach (Order order in updatedList)
        {
            orders.Add(order);
            Debug.Log("updatedList : " + updatedList.Count);
        }
        foreach (Order order in orders)
        {
            Debug.Log("orders : " + orders.Count);
        }
    }
    void Update()
    {
        DisplayOrders();
    }
}
