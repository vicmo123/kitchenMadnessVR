using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTaco : MonoBehaviour
{
    IngredientEnum[] easyRecipes;
    private void Awake()
    {
        easyRecipes = new IngredientEnum[] { IngredientEnum.EasyTaco | IngredientEnum.Sauce, IngredientEnum.BaseOfTaco };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }
    public IngredientEnum GetRecipe()
    {
        //IngredientEnum recipe = easyRecipes[Random.Range(0, 2)];
        return IngredientEnum.BaseOfTaco;
    }
}
