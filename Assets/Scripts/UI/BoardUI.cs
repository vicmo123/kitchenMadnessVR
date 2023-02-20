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
    private float starSpeedRotation = 100;

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
                
            }
        }
    }

    void Update()
    {
        foreach (OrderUI orderUI in ordersUI)
        {
            orderUI.UpdateTimerUI();
        }

        foreach (Transform star in stars)
        {
           
            star.Rotate(0, starSpeedRotation * Time.deltaTime, 0);
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
    public void RemoveOneStar(int currentNbStars)
    {
        if (currentNbStars > 1)
        {
            //Transform starToDestroy = stars[stars.Count - 1].gameObject.SetActive(false);
            stars[currentNbStars - 1].gameObject.SetActive(false);
            //stars.RemoveAt((stars.Count) - 1);
            //GameObject.Destroy(starToDestroy.gameObject);
        }
        else if (currentNbStars == 1)
        {
            //End Of Game!!!!!
            stars[currentNbStars - 1].gameObject.SetActive(false);
        }
    }
    public void EndOfRound()
    {        
        foreach (Transform star in stars)
        {
            star.gameObject.SetActive(true);
        }

        if(orderContainer.childCount > 0)
        {
            for (int i = orderContainer.childCount -1 ; i >= 0; i--)
            {
                Transform child = orderContainer.GetChild(i);
                GameObject.Destroy(child.gameObject);
            }
        }

        ordersUI.Clear();
    }
}