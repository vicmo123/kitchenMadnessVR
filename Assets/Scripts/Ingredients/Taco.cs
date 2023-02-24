using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientReceiver))]
[RequireComponent(typeof(Burnable))]

public class Taco : MonoBehaviour
{
    #region Pivots

    // Pivots to get x,z for ingredients placement
    public Transform pivotMeat;

    public List<Transform> pivotIngredients;

    #endregion

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


    // sauce settings

    public GameObject sauce;
    float maxSauceSize = 0.01f;
    float timeToMaxSauce = 5;
    float currentSauceSize = 0;

    void Start() {
        ingredientReceiver = this.GetComponent<IngredientReceiver>();
        ingredientReceiver.receiverDelegate += AddIngredient;

        burnable = this.GetComponent<Burnable>();
        burnable.grilledDelegate += SetReady;

        ingredientList = new List<Ingredients>();

        sauce.transform.localScale = new Vector3(currentSauceSize, sauce.transform.localScale.y, currentSauceSize);
    }

    public void AddIngredient(Toppingable ingredient) {
        ingredient.gameObject.transform.SetParent(this.gameObject.transform);
        ingredientList.Add(ingredient.ingredientType);
        PlaceIngredients();
    }

    public void PlaceIngredients() {
        int temp = 0;

        for (int i = 0; i < ingredientReceiver.ingredients.Count; i++) {
            SetRotation(ingredientReceiver.ingredients[i]);
            Bounds b = ingredientReceiver.ingredients[i].mf.mesh.bounds;
            if (ingredientReceiver.ingredients[i].ingredientType == Ingredients.Meat) {
                ingredientReceiver.ingredients[i].transform.position = new Vector3(pivotMeat.position.x, pivotMeat.position.y/* + b.extents.y*/, pivotMeat.position.z);
                temp++;
            } else {
                ingredientReceiver.ingredients[i].transform.position = new Vector3(pivotIngredients[i - temp].position.x, pivotIngredients[i - temp].position.y/* + b.extents.y*/, pivotIngredients[i - temp].position.z);
                //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //go.transform.position = ingredientReceiver.ingredients[i].transform.position;
                //go.transform.localScale = b.size/2;
            }
        }
    }

    public void ReceiveSauce() {
        if (this.ingredientReceiver.ready) {
            currentSauceSize += (Time.deltaTime / timeToMaxSauce) * maxSauceSize;
            if (currentSauceSize > maxSauceSize) {
                currentSauceSize = maxSauceSize;
            }
            if (currentSauceSize > maxSauceSize * .1) {
                ingredientList.Add(Ingredients.Sauce);
            }
            sauce.transform.localScale = new Vector3(currentSauceSize, sauce.transform.localScale.y, currentSauceSize);
        }
    }

    private void SetRotation(Toppingable ingredient) {
        Dictionary<string, float> ingRotations = new Dictionary<string, float>();

        //cheese
        ingRotations.Add("polySurface1 Instance", 0);
        ingRotations.Add("polySurface2 Instance", 0);
        //pineapple
        ingRotations.Add("polySurface13 Instance", 0);
        ingRotations.Add("polySurface14 Instance", 180);
        //onion
        ingRotations.Add("polySurface6 Instance", 180);
        ingRotations.Add("polySurface4 Instance", 0);
        //meat
        ingRotations.Add("meatCooked Instance", 0);

        ingredient.transform.rotation = Quaternion.identity;
        ingredient.transform.Rotate(ingRotations[ingredient.mf.mesh.name], 0, 0);
    }

    public IngredientEnum SendTaco() {
        IngredientEnum taco = IngredientEnum.Tortilla;

        if (ingredientList.Contains(Ingredients.Meat))
            taco = taco | IngredientEnum.Meat;
        if (ingredientList.Contains(Ingredients.Cheese))
            taco = taco | IngredientEnum.Cheese;
        if (ingredientList.Contains(Ingredients.Onion))
            taco = taco | IngredientEnum.Onion;
        if (ingredientList.Contains(Ingredients.Pineapple))
            taco = taco | IngredientEnum.Pineapple;
        if (ingredientList.Contains(Ingredients.Sauce))
            taco = taco | IngredientEnum.Sauce;

        return taco | IngredientEnum.Meat;
    }

    public void SetReady() {
        this.ingredientReceiver.ready = true;
        this.gameObject.tag = "Taco";
    }
}
