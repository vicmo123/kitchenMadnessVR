using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientReceiver))]
[RequireComponent(typeof(Burnable))]

public class Taco : MonoBehaviour
{
    // Pivots to get height depending on ingredients quantity
    public GameObject pivotLayer1;
    public GameObject pivotLayer2;

    // Pivots to get x,z depending on ingredients quantity
    public GameObject pivot2_1;
    public GameObject pivot2_2;
    public GameObject pivot3_1;
    public GameObject pivot3_2;
    public GameObject pivot3_3;

    public enum Ingredients
    {
        //tortilla | meat | onion | pineapple | cheese | sauce
        None = 0,    //must have a specified 0
        Tortilla = 1 << 0,    //1
        Meat = 1 << 1,    //2
        Onion = 1 << 2,    //4
        Pineapple = 1 << 3,    //8
        Cheese = 1 << 4,    //16
        Sauce = 1 << 5    //32
    }

    Burnable burnable;

    IngredientReceiver ingredientReceiver;
    List<Ingredients> ingredientList;

    // Start is called before the first frame update
    void Start() {
        ingredientReceiver = this.GetComponent<IngredientReceiver>();
        ingredientReceiver.receiverDelegate += AddIngredient;

        burnable = this.GetComponent<Burnable>();
        burnable.grilledDelegate += SetReady;
        
        ingredientList = new List<Ingredients>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddIngredient(Toppingable ingredient) {
        if (ingredientList.Contains(Ingredients.Meat))
            ingredient.gameObject.transform.SetParent(pivotLayer2.transform);
        else
            ingredient.gameObject.transform.SetParent(pivotLayer1.transform);
        ingredientList.Add(ingredient.ingredientType);
        PlaceIngredients();
    }

    public void PlaceIngredients() {
        if (ingredientList.Contains(Taco.Ingredients.Meat)) {
            ingredientReceiver.GetIngredientOfType(Taco.Ingredients.Meat).transform.localPosition = new Vector3(0, 0, 0);
        }

        int mixedToppingables = 0;
        if (ingredientList.Contains(Taco.Ingredients.Cheese)) {
            mixedToppingables++;
        }
        if (ingredientList.Contains(Taco.Ingredients.Onion)) {
            mixedToppingables++;
        }
        if (ingredientList.Contains(Taco.Ingredients.Pineapple)) {
            mixedToppingables++;
        }

        if (mixedToppingables > 0) {

            List<Toppingable> tempIngredients = GetMixedToppingables();

            if (mixedToppingables == 1) {
                tempIngredients[0].transform.localPosition = new Vector3(0, 0, 0);
            }

            if (mixedToppingables == 2) {
                tempIngredients[0].transform.position = new Vector3(pivot2_1.transform.position.x, tempIngredients[0].transform.parent.transform.position.y, pivot2_1.transform.position.z);
                tempIngredients[1].transform.position = new Vector3(pivot2_2.transform.position.x, tempIngredients[1].transform.parent.transform.position.y, pivot2_2.transform.position.z);
            }

            if (mixedToppingables == 3) {
                tempIngredients[0].transform.position = new Vector3(pivot3_1.transform.position.x, tempIngredients[0].transform.parent.transform.position.y, pivot3_1.transform.position.z);
                tempIngredients[1].transform.position = new Vector3(pivot3_2.transform.position.x, tempIngredients[1].transform.parent.transform.position.y, pivot3_2.transform.position.z);
                tempIngredients[2].transform.position = new Vector3(pivot3_3.transform.position.x, tempIngredients[2].transform.parent.transform.position.y, pivot3_3.transform.position.z);
            }

            // INCLUDE SAUCE
        }

    }

    private List<Toppingable> GetMixedToppingables() {
        List<Toppingable> mixedIngredients = new List<Toppingable>();

        Toppingable tempIngredient = ingredientReceiver.GetIngredientOfType(Taco.Ingredients.Cheese);
        if (tempIngredient != null) {
            mixedIngredients.Add(tempIngredient);
        }
        tempIngredient = ingredientReceiver.GetIngredientOfType(Taco.Ingredients.Onion);
        if (tempIngredient != null) {
            mixedIngredients.Add(tempIngredient);
        }

        tempIngredient = ingredientReceiver.GetIngredientOfType(Taco.Ingredients.Pineapple);
        if (tempIngredient != null) {
            mixedIngredients.Add(tempIngredient);
        }

        return mixedIngredients;
    }

    public Ingredients SendTaco() {
        Ingredients taco = Ingredients.None;
        for (int i = 1; i < Enum.GetNames(typeof(Ingredients)).Length; i++) {
            if (ingredientList.Contains((Ingredients)i)) {
                if (i == 1) {
                    taco = (Ingredients)i;
                }
                else {
                    taco = taco | (Ingredients)i;
                }
            }
        }

        return taco;
    }

    public void SetReady() {
        this.ingredientReceiver.ready = true;
    }
}
