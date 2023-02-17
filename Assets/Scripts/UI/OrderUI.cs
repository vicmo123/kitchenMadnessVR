using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class OrderUI : MonoBehaviour
{
    private bool DEBUG_MODE = false;

    public Transform orderContainer;

    private List<Transform> ingredients = new List<Transform>();
    private RectTransform timer;
    private Image timerImg;
    private Transform ingredientContainer;
    private Order order;
    Dictionary<IngredientEnum, GameObject> ingredientsToDisplay = new Dictionary<IngredientEnum, GameObject>();

    private void Awake()
    {
        ingredientContainer = transform.GetChild(0);
        int nbChild = ingredientContainer.childCount;
        for (int i = 0; i < nbChild; i++)
        {
            GameObject go = ingredientContainer.GetChild(i).gameObject;
            string ingredient = go.GetComponent<Image>().sprite.name.ToString();
            ingredientsToDisplay.Add((IngredientEnum)Enum.Parse(typeof(IngredientEnum), ingredient), go);
        }

        //Get all the images of ingredients and set them to non visible
        ingredientContainer = transform.GetChild(0);
        for (int i = 0; i < ingredientContainer.childCount; i++)
        {
            Transform image = ingredientContainer.GetChild(i);
            image.gameObject.SetActive(false);
            ingredients.Add(image);
        }

        timer = transform.GetChild(1).GetComponent<RectTransform>();
        timerImg  = timer.GetComponent<Image>();
        timerImg.color = Color.green;
    }

    public void SetIngredientVisible(IngredientEnum recipe)
    {
        foreach (KeyValuePair<IngredientEnum, GameObject> entry in ingredientsToDisplay)
        {
            if ((recipe & entry.Key) == entry.Key)
            {
                entry.Value.SetActive(true);
            }
        }
    }

    public void UpdateTimerUI()
    {
        timer.localScale = new Vector3(order.PourcentageLeft, 1, 1);

        if (order.PourcentageLeft > .5)
        {
            timerImg.color = Color.green;
        }else if (order.PourcentageLeft >.2)
        {
            timerImg.color = Color.yellow;
        }else if (order.PourcentageLeft <= .20)
        {
            timerImg.color = Color.red;
        }
    }
    

    public void SetOrder(Order order)
    {
        this.order = order;
    }

    public int GetId()
    {
        return this.order.GetId();
    }
}
