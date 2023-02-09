using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientReceiver))]

public class Taco : MonoBehaviour
{
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

    IngredientReceiver ingredientReceiver;
    List<Ingredients> ingredientList;

    // Start is called before the first frame update
    void Start() {
        ingredientReceiver = this.GetComponent<IngredientReceiver>();
        ingredientReceiver.receiverDelegate += AddIngredient;

        ingredientList = new List<Ingredients>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void AddIngredient(Toppingable ingredient) {
        ingredient.gameObject.transform.SetParent(this.transform);
        ingredientList.Add(ingredient.ingredientType);
        PlaceIngredients();
    }

    public void PlaceIngredients() {
        float heightForNextIngredient = 0;

        if (ingredientList.Contains(Taco.Ingredients.Meat)) {
            heightForNextIngredient += 1.3f;
            ingredientReceiver.GetIngredientOfType(Taco.Ingredients.Meat).transform.localPosition = new Vector3(0, heightForNextIngredient, 0);
        }

        heightForNextIngredient += 23;

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
                tempIngredients[0].transform.localPosition = new Vector3(0, heightForNextIngredient, 0);
                heightForNextIngredient += 0.1f;
            }

            if (mixedToppingables == 2) {
                tempIngredients[0].transform.localPosition = new Vector3(0, heightForNextIngredient, .25f);
                tempIngredients[1].transform.localPosition = new Vector3(0, heightForNextIngredient, -.25f);
            }

            if (mixedToppingables == 3) {
                tempIngredients[0].transform.localPosition = new Vector3(-.25f, heightForNextIngredient, 0);
                tempIngredients[1].transform.localPosition = new Vector3(.15f, heightForNextIngredient, -.2f);
                tempIngredients[2].transform.localPosition = new Vector3(.15f, heightForNextIngredient, .2f);
            }

            heightForNextIngredient += .1f;

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
                } else {
                    taco = taco | (Ingredients)i;
                }
            }
        }

        return taco;
    }
}
