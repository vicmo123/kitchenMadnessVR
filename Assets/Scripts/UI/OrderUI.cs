using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OrderUI : MonoBehaviour
{
    public Transform OrderContainer;
    public GameObject orderSlot_Prefab;
    public GameObject timer_Prefab;
    public GameObject ingredientContainer_Prefab;

    private List<GameObject> ingredients = new List<GameObject>();
    private Transform orderSlot;
    private RectTransform timer;
    private Transform ingredientContainer;

    private void Awake()
    {
        GameObject.Instantiate(orderSlot_Prefab).transform.SetParent(OrderContainer);
        orderSlot = OrderContainer.GetChild(0);

        GameObject.Instantiate(ingredientContainer_Prefab).transform.SetParent(orderSlot);
        ingredientContainer = orderSlot.GetChild(0);

        GameObject.Instantiate(timer_Prefab).transform.SetParent(orderSlot);
        timer = orderSlot.GetChild(1).GetComponent<RectTransform>();
    }

    public void AddIngredientImage(GameObject ingredientPrefab)
    {
        ingredients.Add(ingredientPrefab);
    }

    private void SetImagesToParent()
    {
        foreach (GameObject image in ingredients)
        {
            GameObject.Instantiate(image).transform.SetParent(ingredientContainer);
        }
    }
    
    public void UpdateTimerUI(float pourcentage)
    {
        timer.localScale = new Vector3(pourcentage, 1, 1);
    }
}
