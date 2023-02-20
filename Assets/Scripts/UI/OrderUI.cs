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
    private Transform cross;
    private Image timerImg;
    private Transform ingredientContainer;
    private Image bakcgroundImg;
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

        //Get all the images of ingredients and set them to unvisible
        ingredientContainer = transform.GetChild(0);
        for (int i = 0; i < ingredientContainer.childCount; i++)
        {
            Transform image = ingredientContainer.GetChild(i);
            image.gameObject.SetActive(false);
            ingredients.Add(image);
        }

        cross = transform.GetChild(2);
        cross.gameObject.SetActive(false);

        timer = transform.GetChild(1).GetComponent<RectTransform>();
        timerImg = timer.GetComponent<Image>();
        timerImg.color = Color.green;

        bakcgroundImg = ingredientContainer.GetComponent<Image>();
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
        }
        else if (order.PourcentageLeft > .2)
        {
            timerImg.color = Color.yellow;
        }
        else if (order.PourcentageLeft <= .20)
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


    public IEnumerator GoodJobEffect()
    {
        float startTime = Time.time;
        float timeEffet = Time.time + 3;
        Image img = transform.GetComponent<Image>();
        img.color = new Color(.13f, .89f, .16f, .78f);

        while (Time.time < timeEffet)
        {
            StartCoroutine(ScaleUp(2, startTime));
            StartCoroutine(ScaleDown(1, startTime + 2));
            yield return null;
        }
        GameObject.Destroy(gameObject);
    }

    public IEnumerator CrossEffect()
    {
        cross.gameObject.SetActive(true);
        float startTime = Time.time;
        float timeEffet = Time.time + 3;
        bakcgroundImg.color = new Color(1, .13f, .19f, .78f);

        while (Time.time < timeEffet)
        {
            StartCoroutine(ScaleUp(2, startTime));
            StartCoroutine(ScaleDown(1, startTime + 2));
            yield return null;
        }
        GameObject.Destroy(gameObject);
    }

    IEnumerator ScaleDown(float timeEffet, float startTime)
    {
        SoundManager.MoveOrder.Invoke();
        while (transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.Lerp(new Vector3(1.3f, 1.3f, 1.3f), Vector3.zero, Time.time - startTime / timeEffet );
            yield return null;
        }
    }

    IEnumerator ScaleUp(float timeEffet, float startTime)
    {
        while (transform.localScale != Vector3.zero)
        {
            transform.localScale = Vector3.Lerp( Vector3.one, new Vector3(1.25f, 1.25f, 1.25f), Time.time - startTime / timeEffet);
            yield return null;
        }
    }
}
